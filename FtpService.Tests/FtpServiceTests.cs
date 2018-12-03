using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace FtpService.Tests
{
    public class FtpServiceTests
    {
        [Fact]
        public async Task FtpConnectionTest()
        {
            string url = "ftp://waws-prod-am2-165.ftp.azurewebsites.windows.net/site/test";
            string login = @"renderhubserver\RenderHubAdmin";
            string psw = @"7j9u*@jyB29729S9";
            var ftpService = new FtpService(url,login,psw);

            string[] fileName = await ftpService.GetFileList();
            Assert.NotEmpty(fileName);
        }

        [Fact]
        public async Task FtpDownloadFileTest()
        {
            string url = "ftp://waws-prod-am2-165.ftp.azurewebsites.windows.net/site/test";
            string login = @"renderhubserver\RenderHubAdmin";
            string psw = @"7j9u*@jyB29729S9";
            var ftpService = new FtpService(url, login, psw);
            string fileName = "1234567890123456_123456789.zip";

            Stream file = await ftpService.ReadFile(fileName);
            Assert.NotNull(file);

            var zipService = new ZipServcie();
            var archivedFiles = zipService.UnzipFile(file);

            Assert.NotEmpty(archivedFiles);            
            
        }
    }
}
