using DbService;
using FtpService;
using Models;
using StorageService;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class FtpLogic
    {
        private readonly FtpMonitoringService _ftp;
        private readonly DataService _db;
        private readonly ZipServcie _zip;
        private readonly BlobStorageService _storage;

        public FtpLogic(FtpMonitoringService ftp, DataService db,  ZipServcie zip, BlobStorageService storage)
        {
            _ftp = ftp;
            _db = db;
            _zip = zip;
            _storage = storage;
        }

        public async Task CheckNewRequests()
        {
            string[] fileNames = await _ftp.GetFileList();
            var zipFiles = fileNames.Where(name => name.EndsWith(".zip"));
            var requests = await _db.SaveNewRequest(zipFiles);

            foreach (var request in requests)
            {
                await UnzipAndStoreRequest(request);
            }
        }

        private async Task UnzipAndStoreRequest(RequestPackage request)
        {
            Stream downloadedFile = await _ftp.ReadFile(request.ZipFileName);
            foreach (var zipEntry in _zip.UnzipFile(downloadedFile))
            {
                await _storage.SaveFileToBlob(zipEntry.Name, zipEntry.Open());

                var record = new RecordFile()
                {
                    FileName = zipEntry.Name,
                    Status = RecordStatusEnum.SavedToBlobStorage,
                    RequestPackageId = request.Id
                };
                await _db.SaveNewRecord(record);
                if (zipEntry.Name.EndsWith(".pdf"))
                {
                    request.DeatilsFileName = zipEntry.Name;
                    request.DetailsRecordId = record.Id;
                }
            }
            await _db.UpdateRequestPackage(request);
            await _ftp.MoveFileToProcessed(request.ZipFileName);
        }
    }
}
