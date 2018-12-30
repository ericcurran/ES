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
        private readonly FtpClient _ftpClient;
        private readonly ILogger<AppLogic> _log;

        public AppLogic(DataService db, BlobStorageService bs, FtpClient ftpClient, ILogger<AppLogic> log)
        {
            _db = db;
            _ftpClient = ftpClient;
            _log = log;
        }

        public async Task Run()
        {
            _log.LogInformation($"Job started!");

            var newZipFileName = await GetZipFilesFromFtp();
            
            foreach (var zip in newZipFileName)
            {
                _log.LogInformation($"Start processing file {zip.ZipFileName}");
                
                var downloadedFile = await DownloadfromFtp(zip);

                var zipEntires = GetZipEntries(downloadedFile);

                foreach (var record in zipEntires)
                {
                    //recordToSave = zipRecord.Open();
                    // log.LogInformation($"file {zipRecord.Name} started to save in blob");
                    await SaveRecordAndUpdateRequest(new RecordFile()
                    {
                        FileName = record.Name,
                        RequestPackageId = zip.Id,
                        Status = RecordStatusEnum.SavedToBlobStorage
                    });
                }
                await CleanFtp(zip);
                _log.LogInformation($"Finished processing file {zip.ZipFileName}");
            }

                _log.LogInformation($"Job ended!");
        }

        public async Task<List<RequestPackage>> GetZipFilesFromFtp()
        {
            string[] fileNames = await _ftpClient.GetFileList();
            var zipFiles       = fileNames.Where(name => name.EndsWith(".zip"));
            var newZipFIles    = await _db.SaveNewRequests(zipFiles);
            return newZipFIles.ToList();
        }

        public async Task<Stream> DownloadfromFtp(RequestPackage request)
        {
            Stream downloadedFile = await _ftpClient.ReadFile(request.ZipFileName);
            return downloadedFile;
        }

        public async Task SaveRecordAndUpdateRequest(RecordFile record)
        {
           await _db.SaveNewRecordAndUpdaterequest(record);            
        }

        public static IEnumerable<ZipArchiveEntry> GetZipEntries(Stream downloadedFile)
        {
            return ZipServcie.UnzipFile(downloadedFile);
        }

        public  async Task CleanFtp(RequestPackage zipFile)
        {
            await _ftpClient.MoveFileToProcessed(zipFile.ZipFileName);
            await _db.UpdateRequestPackagetoStatus(zipFile.Id, RequestStatusEnum.SavedToBlob);
        }

    }
}
