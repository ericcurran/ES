using System;
using System.Threading.Tasks;
using Xunit;

namespace StorageService.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task ConnectionTest()
        {
            var connString = "DefaultEndpointsProtocol=https;AccountName=casedocuments;AccountKey=jhAd1rdcohb6ZnPfHvcNvMLHpGUWDDN7/P2z2fBu3peQg/JZfnya49Xrv840DBwGLdhRlgt/1oXxD39Btrr5Lw==;EndpointSuffix=core.windows.net";
            var service = new StorageService(connString);

            await service.ProcessAsync();

        }
    }
}
