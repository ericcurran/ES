//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Xunit;

//namespace FtpService.Tests
//{
//    public class FtpServiceTests
//    {
//        [Fact]
//        public async Task FtpConnectionTest()
//        {
//            string url = "ftp://waws-prod-am2-165.ftp.azurewebsites.windows.net/site/test";
//            string login = @"renderhubserver\RenderHubAdmin";
//            string psw = @"7j9u*@jyB29729S9";
//            string dir = "processed";
//            var ftpService = new FtpMonitoringService(url,login,psw, dir);

//            string[] fileName = await ftpService.GetFileList();
//            Assert.NotEmpty(fileName);
//        }

//        [Fact]
//        public async Task FtpRenameTest()
//        {
//            string url = "ftp://waws-prod-am2-165.ftp.azurewebsites.windows.net/site/test";
//            string login = @"renderhubserver\RenderHubAdmin";
//            string psw = @"7j9u*@jyB29729S9";
//            string dir = "processed";
//            string fileToRename = "0138739400101059_092748059.zip";
//            var ftpService = new FtpMonitoringService(url, login, psw, dir);

//            await ftpService.MoveFileToProcessed(fileToRename);
//            //Assert.NotEmpty(fileName);
//        }

//        [Fact]
//        public async Task FtpDownloadFileTest()
//        {
//            string url = "ftp://waws-prod-am2-165.ftp.azurewebsites.windows.net/site/test";
//            string login = @"renderhubserver\RenderHubAdmin";
//            string psw = @"7j9u*@jyB29729S9";
//            var ftpService = new FtpMonitoringService(url, login, psw, null);
//            string fileName = "1234567890123456_123456789.zip";

//            Stream file = await ftpService.ReadFile(fileName);
//            Assert.NotNull(file);

//            var zipService = new ZipServcie();
//            var archivedFiles = zipService.UnzipFile(file);

//            Assert.NotEmpty(archivedFiles);            
            
//        }
//    }
//}
