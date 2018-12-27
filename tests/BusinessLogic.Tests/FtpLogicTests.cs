using DbService;
using FtpService;
using Microsoft.EntityFrameworkCore;
using StorageService;
using System;
using System.Threading.Tasks;
using Xunit;

namespace BusinessLogic.Tests
{
    public class FtpLogicTests
    {
        //[Fact]
        //public async Task SaveNewFilesTest()
        //{
        //    var ftpService = GetFtpService();
            
        //    var storage = GetBlobStorage();
        //    var logic = new FtpLogic(ftpService, dbService, new ZipServcie(), storage);

        //    await logic.GetZipFilesFromFtp();
        //}

        //private BlobStorageService GetBlobStorage()
        //{
        //    string _connectionString = "DefaultEndpointsProtocol=https;AccountName=casedocuments;AccountKey=jhAd1rdcohb6ZnPfHvcNvMLHpGUWDDN7/P2z2fBu3peQg/JZfnya49Xrv840DBwGLdhRlgt/1oXxD39Btrr5Lw==;EndpointSuffix=core.windows.net";
        //    string _containerName = "documents-test";
        //    return new BlobStorageService(_connectionString, _containerName);
        //}

        //private FtpMonitoringService GetFtpService()
        //{
        //    string url = "ftp://waws-prod-am2-165.ftp.azurewebsites.windows.net/site/test";
        //    string login = @"renderhubserver\RenderHubAdmin";
        //    string psw = @"7j9u*@jyB29729S9";
        //    string dir = "processed";
        //    var ftpService = new FtpMonitoringService(url, login, psw, dir);
        //    return ftpService;
        //}
    }
}
