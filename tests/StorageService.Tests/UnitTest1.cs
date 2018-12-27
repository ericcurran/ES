//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Xunit;

//namespace StorageService.Tests
//{
//    public class StorageServiceTests
//    {
//        private readonly string _connectionString = "DefaultEndpointsProtocol=https;AccountName=casedocuments;AccountKey=jhAd1rdcohb6ZnPfHvcNvMLHpGUWDDN7/P2z2fBu3peQg/JZfnya49Xrv840DBwGLdhRlgt/1oXxD39Btrr5Lw==;EndpointSuffix=core.windows.net";
//        private readonly string _containerName = "documents-test";
//        [Fact]
//        public async Task ContainerExistTest()
//        {
            
//            var service = new BlobStorageService(_connectionString, _containerName);

//            bool exist = await service.IsContainerExist();

//            Assert.True(exist);
//        }

//        [Fact]
//        public async Task ContainerListItemsTest()
//        {

//            var service = new BlobStorageService(_connectionString, _containerName);

//            var items = await service.GetContainerItems();
//            items.FirstOrDefault();

//            Assert.NotEmpty(items);
//        }

//        [Fact]
//        public async Task IsDirectoryExistTest()
//        {

//            var service = new BlobStorageService(_connectionString, _containerName);
                       
//            Assert.True(await service.IsDirectoryExist("123"));
//            Assert.False(await service.IsDirectoryExist("12sw3"));
//        }
//    }
//}
