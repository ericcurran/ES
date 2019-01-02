using DbService;
using FtpService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StorageService;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BusinessLogic.Tests
{
    public class FtpLogicTests
    {
        private readonly LoggerFactory _loggerFactory;

        public FtpLogicTests(ITestOutputHelper testOutputHelper)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new XunitLoggerProvider(testOutputHelper));
            _loggerFactory = loggerFactory;
        }

        [Fact]
        public async Task SaveNewFilesTest()
        {
            var dsLog = GetLogger<DataService>();
            var ds = new DataService("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RequestManagmentDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False", dsLog);
            var ftpService = GetFtpClient();
            var log = GetLogger<AppLogic>();

            var storage = GetBlobStorage();
            var logic = new AppLogic(ds, storage, ftpService, log);

            await logic.Run();
        }

        private BlobStorageService GetBlobStorage()
        {
            string _connectionString = "DefaultEndpointsProtocol=https;AccountName=casedocuments;AccountKey=jhAd1rdcohb6ZnPfHvcNvMLHpGUWDDN7/P2z2fBu3peQg/JZfnya49Xrv840DBwGLdhRlgt/1oXxD39Btrr5Lw==;EndpointSuffix=core.windows.net";
            string _containerName = "documents-test";
            var log = GetLogger<BlobStorageService>();
            return new BlobStorageService(_connectionString, _containerName, log);
        }

        private FtpClient GetFtpClient()
        {
            string url = "ftp://waws-prod-am2-165.ftp.azurewebsites.windows.net/site/test";
            string login = @"renderhubserver\RenderHubAdmin";
            string psw = @"7j9u*@jyB29729S9";
            string dir = "processed";
            var logger = GetLogger<FtpClient>();
            var ftp = new FtpClient(url, login, psw, dir, logger);
            return ftp;
        }

        private ILogger<T> GetLogger<T>()
        {

            var logger = _loggerFactory.CreateLogger<T>();
            return logger;
        }
    }
}
