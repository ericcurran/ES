using DbService;
using FtpService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Models;
using StorageService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Timers = System.Timers;

namespace BusinessLogic
{
    public class AppLogic: IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppLogic> _log;
        private readonly Timers.Timer _timerToStartJob;

        private bool _jobRunning;
        private DataService _db;
        private BlobStorageService _bs;
        private FtpClient _ftpClient;

        public AppLogic(IServiceProvider serviceProvider, ILogger<AppLogic> log)
        {
            _serviceProvider = serviceProvider;          
            _log = log;

            _timerToStartJob = new Timers.Timer
            {
                AutoReset = true,
                Interval = TimeSpan.FromMinutes(5).TotalMilliseconds
            };
            _timerToStartJob.Elapsed += ProccessFilesTimedEvent;
        }

        public async Task Run()
        {
            _jobRunning = true;
            using (var scope = _serviceProvider.CreateScope())
            {
                _db = scope.ServiceProvider.GetService<DataService>();
                _bs = scope.ServiceProvider.GetService<BlobStorageService>();
                _ftpClient = scope.ServiceProvider.GetService<FtpClient>();

                _log.LogInformation($"Job started!");

                var requests = await GetZipFileList();

                foreach (var request in requests)
                {
                    _log.LogInformation($"Start processing file {request.GetZipFileName()}");

                    var downloadedFile = await DownloadfromFtp(request.GetZipFileName());
                    if (downloadedFile == null)
                    {
                        continue;
                    }

                    var zipEntires = await GetZipEntries(downloadedFile);
                    var sameClaimFileNames = await _db.GetFileNamesByClaimNumber(request.ClaimNumber);
                    var savedRecords = await SaveToBlob(zipEntires, request.Id, sameClaimFileNames);
                    await SaveRecordAndUpdateRequest(savedRecords);

                    await CleanFtp(request);
                    _log.LogInformation($"Finished processing file {request.GetZipFileName()}");
                }

                _log.LogInformation($"Job ended!");

            }
            _jobRunning = false;
            _db = null;
            _bs = null;
            _ftpClient = null;
        }

        private async Task<List<Record>> SaveToBlob(IEnumerable<ZipEntryModel> records, int requestId, IEnumerable<string> sameClaimFileNames)
        {

            var savedRecords = new List<Record>();
            var savedTasks = new List<(Task<string> task, bool isDuplicated, string originFileName)>();
            Task whenAllTask;
            foreach (var record in records)
            {
                var isDuplicated = false;
                var originFileName = record.FileName;
                if (sameClaimFileNames.Contains(record.FileName))
                {
                    record.FileName = GetUniqueFileName(record.FileName);
                    isDuplicated = true;
                }
                var task = _bs.SaveFileToBlob(record.FileName, record.FileStrem);
                savedTasks.Add((task, isDuplicated, originFileName));
                if (savedTasks.Count == 50)
                {
                    whenAllTask = Task.WhenAll(savedTasks.Select(d=>d.task));
                    await whenAllTask;
                    AddFileRecords(requestId, savedRecords, savedTasks);
                    savedTasks.Clear();
                }
            }
            if (savedTasks.Count > 0)
            {
                await Task.WhenAll(savedTasks.Select(d => d.task));
                AddFileRecords(requestId, savedRecords, savedTasks);
            }
            return savedRecords;
        }

        private string GetUniqueFileName(string fileName)
        {
            if(fileName.EndsWith("_1-0.pdf"))
            {
                var substring = fileName.Substring(0, fileName.Length - 8);
                return $"{substring}_{DateTime.Now.Ticks}_1-0.pdf";
            }
            var data = fileName.Split('.');
            return $"{data}_{DateTime.Now.Ticks}.{data[1]}";
        }

        private void AddFileRecords(int requestId, 
                                    List<Record> savedRecords, 
                                    List<(Task<string> task, bool isDuplicated, string orignalFileName)> savedTasks)
        {
            foreach (var t in savedTasks)
            {
                AddRecordToSave(requestId, savedRecords, t.task.Result, t.isDuplicated, t.orignalFileName);
            }
        }

        private static void AddRecordToSave(int requestId, List<Record> savedRecords, string fileName, bool isDuplicate, string originalFileName)
        {
            var fileNameData = new RecordFileName(originalFileName);
            var recordFile = new Record();
            recordFile.FileName = fileName;
            recordFile.RequestId = requestId;
            recordFile.ClaimNumber = fileNameData.ClaimNumber;
            recordFile.BundleNumber = fileNameData.BundleNumber;
            recordFile.PageNumber = fileNameData.PageNumber;
            recordFile.OrderNumber = fileNameData.OrderNumber;
            recordFile.Status = RecordStatusEnum.SavedToAzure;
            recordFile.Duplicated = isDuplicate;
            savedRecords.Add(recordFile);
        }

        public async Task<List<Request>> GetZipFileList()
        {
            string[] fileNames = await _ftpClient.GetFileList();
            var zipFiles       = fileNames.Where(name => name.EndsWith(".zip"));
            _log.LogInformation($"Found {zipFiles.Count()} zip files");

            var requests = zipFiles
                .Select(GetClaimNumAndRequestId)
                .Select(fileData => new Request
                {
                    ClaimNumber = fileData.claimNumber,
                    RequestId   = fileData.requestId,
                    Type        = RequestTypeEnum.Peer,
                    Created     = DateTime.Now,
                }).ToList();
            var newZipFIles    = await _db.SaveNewRequests(requests);
            _log.LogInformation($"Info about {newZipFIles.Count()} zip files saved to database");
            return newZipFIles.ToList();
        }

        private (string claimNumber, string requestId) GetClaimNumAndRequestId(string fileName)
        {
            var fn = fileName.Substring(0, fileName.Length - 4);
            var data = fn.Split('_');
            return (data[0], data[1]);
        }

        public async Task<Stream> DownloadfromFtp(string zipFileName)
        {
            Stream downloadedFile = await _ftpClient.ReadFile(zipFileName);
            return downloadedFile;
        }

        public async Task SaveRecordAndUpdateRequest(IEnumerable<Record> records)
        {
           await _db.SaveNewRecordAndUpdaterequest(records);            
        }

        public static async Task<IEnumerable<ZipEntryModel>> GetZipEntries(Stream downloadedFile)
        {
            return await ZipServcie.UnzipFile(downloadedFile);
        }

        public  async Task CleanFtp(Request request)
        {
            await _ftpClient.MoveFileToProcessed(request.GetZipFileName());
            await _db.UpdateRequestPackagetoStatus(request.Id, RequestStatusEnum.SavedToAzure);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timerToStartJob.Enabled = true;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _timerToStartJob.Enabled = false;
        }

        private async void ProccessFilesTimedEvent(object sender, Timers.ElapsedEventArgs e)
        {
            if (_jobRunning)
            {            
                return;
            }
            await Task.WhenAll(Run());
        }
    }
}
