using DbService;
using FtpService;
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
    public static class FtpLogic
    {       

        public static async Task<IEnumerable<RequestPackage>> GetZipFilesFromFtp()
        {
            string[] fileNames = await FtpMonitoringService.GetFileList();
            var zipFiles       = fileNames.Where(name => name.EndsWith(".zip"));
            var newZipFIles    = await DataService.SaveNewRequests(zipFiles);
            return newZipFIles;
        }

        public static async Task<Stream> DownloadfromFtp(RequestPackage request)
        {
            Stream downloadedFile = await FtpMonitoringService.ReadFile(request.ZipFileName);
            return downloadedFile;
        }

        public static async Task SaveRecordAndUpdateRequest(RecordFile record)
        {
           await DataService.SaveNewRecordAndUpdaterequest(record);            
        }

        public static IEnumerable<ZipArchiveEntry> GetZipEntries(Stream downloadedFile)
        {
            return ZipServcie.UnzipFile(downloadedFile);
        }

        public  static async Task CleanFtp(RequestPackage zipFile)
        {
            await FtpMonitoringService.MoveFileToProcessed(zipFile.ZipFileName);
            await DataService.UpdateRequestPackagetoStatus(zipFile.Id, RequestStatusEnum.SavedToBlob);
        }

    }
}
