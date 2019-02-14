using DbService;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RecordsManagmentWeb.Controllers;
using RecordsManagmentWeb.Services;
using StorageService;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Web.Tests
{
    public class PdfGenerationTest
    {
        [Fact]
        public async Task Test1PdfGeneration()
        {
            var nodeMock = new Mock<INodeServices>();

            var connString = $"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RequestManagmentDb;" +
                $"Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;" +
                $"ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            var opt = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlServer(connString)
                        .Options;
            var context = new ApplicationDbContext(opt);

            var blobStorageConnString = $"DefaultEndpointsProtocol=https;AccountName=casedocuments;" +
                $"AccountKey=jhAd1rdcohb6ZnPfHvcNvMLHpGUWDDN7/P2z2fBu3peQg/JZfnya49Xrv840DBwGLdhRlgt/1oXxD39Btrr5Lw==;" +
                $"EndpointSuffix=core.windows.net";
            var blobContainer = "documents-test";

            var loggerMock = new Mock<ILogger<BlobStorageService>>();

            var blobService = new BlobStorageService(blobStorageConnString, blobContainer, loggerMock.Object) ;

            string tempRecordPath = "C:\\Temp\\Records\\";

            var service = new PdfGenearatorService(nodeMock.Object, context,blobService,tempRecordPath);
            var token = new CancellationToken();
            var packId = 8002;

            await service.GeneratePdf(token, packId);

        }
    }
}
