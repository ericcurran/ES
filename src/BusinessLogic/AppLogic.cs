using DbService;
using FtpService;
using Microsoft.Extensions.Logging;
using Models;
using StorageService;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class AppLogic
    {
        private readonly DataService _db;
        private readonly BlobStorageService _bs;
        private readonly FtpClient _ftpClient;
        private readonly ILogger<AppLogic> _log;

        public AppLogic(DataService db, BlobStorageService bs, FtpClient ftpClient, ILogger<AppLogic> log)
        {
            _db = db;
            _bs = bs;
            _ftpClient = ftpClient;
            _log = log;
        }

        public async Task Run()
        {
            _log.LogInformation($"Job started!");

            var newZipFileName = await GetZipFileList();
            
            foreach (var zip in newZipFileName)
            {
                _log.LogInformation($"Start processing file {zip.ZipFileName}");
                
                var downloadedFile = await DownloadfromFtp(zip);
                if (downloadedFile == null)
                {
                    continue;
                }

                var zipEntires = await GetZipEntries(downloadedFile);
                                
                var savedRecords = await SaveToBlob(zipEntires, zip.Id);
                await SaveRecordAndUpdateRequest(savedRecords);

                await CleanFtp(zip);
                _log.LogInformation($"Finished processing file {zip.ZipFileName}");
            }

                _log.LogInformation($"Job ended!");
        }

        private async Task<List<RecordFile>> SaveToBlob(IEnumerable<ZipEntryModel> records, int zipId)
        {

            var savedRecords = new List<RecordFile>();
            var savedTasks = new List<Task<RecordFile>>();
            Task whenAllTask = null;
            foreach (var record in records)
            {
                var task = _bs.SaveFileToBlob(record.FileName, record.FileStrem);
                savedTasks.Add(task);
                if (savedTasks.Count == 50)
                {
                    whenAllTask = Task.WhenAll(savedTasks);
                    await whenAllTask;
                    savedTasks.ForEach(t =>
                    {
                        t.Result.RequestPackageId = zipId;
                        savedRecords.Add(t.Result);
                    });
                    savedTasks.Clear();
                }
            }
            if (savedTasks.Count > 0)
            {
                await Task.WhenAll(savedTasks);
                savedTasks.ForEach(t =>
                {
                    t.Result.RequestPackageId = zipId;
                    savedRecords.Add(t.Result);
                });
            }

            return savedRecords;
        }

        public async Task<List<RequestPackage>> GetZipFileList()
        {
            string[] fileNames = await _ftpClient.GetFileList();
            var zipFiles       = fileNames.Where(name => name.EndsWith(".zip"));
            _log.LogInformation($"Found {zipFiles.Count()} zip files");
            var newZipFIles    = await _db.SaveNewRequests(zipFiles);
            _log.LogInformation($"Info about {newZipFIles.Count()} zip files saved to database");
            return newZipFIles.ToList();
        }

        public async Task<Stream> DownloadfromFtp(RequestPackage request)
        {
            Stream downloadedFile = await _ftpClient.ReadFile(request.ZipFileName);
            return downloadedFile;
        }

        public async Task SaveRecordAndUpdateRequest(IEnumerable<RecordFile> records)
        {
           await _db.SaveNewRecordAndUpdaterequest(records);            
        }

        public static async Task<IEnumerable<ZipEntryModel>> GetZipEntries(Stream downloadedFile)
        {
            return await ZipServcie.UnzipFile(downloadedFile);
        }

        public  async Task CleanFtp(RequestPackage zipFile)
        {
            await _ftpClient.MoveFileToProcessed(zipFile.ZipFileName);
            await _db.UpdateRequestPackagetoStatus(zipFile.Id, RequestStatusEnum.SavedToBlob);
        }

    }
}
