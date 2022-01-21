namespace Azure.Services.Interaction.Storage.Test
{
    using Azure.Services.Interaction.Storage.Configuration;
    using Azure.Services.Interaction.Storage.Services;
    using Azure.Services.Interaction.Storage.Test.Common;
    using Azure.Services.Interaction.Storage.Test.Helper;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class AzureTableStorageServiceTest
    {
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private IOptions<AzureTableStorageOptions> _serviceTableStorageOptions;

        private const string GuidId = "988E4AF7-B42C-46E0-BDE4-6EBCE9A77DF8";

        private readonly Mock<CloudTableMock> _cloudTableMock;
        public AzureTableStorageServiceTest()
        {
            _loggerFactoryMock = new Mock<ILoggerFactory>();

            _serviceTableStorageOptions = Options.Create<AzureTableStorageOptions>(new AzureTableStorageOptions
            {                
                ConnectionString = "BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;"
            });

            _cloudTableMock = new Mock<CloudTableMock>();
            _cloudTableMock.Setup(m => m.CreateIfNotExistsAsync());
            _cloudTableMock
                .Setup(m => m.ExecuteAsync(It.IsAny<TableOperation>()))
                .Returns(Task.FromResult(new TableResult() {                     
                    Etag = GuidId, 
                    HttpStatusCode = 200, 
                    Result = GetStaticTestData() 
                }));
        }

        [Fact]
        public async void Store_Table_Information_Successfully()
        {
            var service = new AzureTableStorageService<ItemTransaction>(_loggerFactoryMock.Object, _serviceTableStorageOptions, _cloudTableMock.Object);

            await service.AddOrUpdateAsync(GetStaticTestData());
            
            Assert.True(true);
        }

        [Fact]
        public async void Delete_Table_Information_Successfully()
        {
            var service = new AzureTableStorageService<ItemTransaction>(_loggerFactoryMock.Object, _serviceTableStorageOptions, _cloudTableMock.Object);
            
            await service.AddOrUpdateAsync(GetStaticTestData());

            await service.DeleteAsync(GetStaticTestData());

            Assert.True(true);
        }

        [Fact]
        public async void Get_Table_Information_Successfully()
        {
            var service = new AzureTableStorageService<ItemTransaction>(_loggerFactoryMock.Object, _serviceTableStorageOptions, _cloudTableMock.Object);

            await service.AddOrUpdateAsync(GetStaticTestData());

            var data = await service.GetAsync(Constants.PartitionKey, GuidId);

            Assert.NotNull(data);
        }

        private static ItemTransaction GetTestData()
        {
            return new ItemTransaction(Guid.NewGuid().ToString(), $"{Guid.NewGuid()}.txt", "CREATED");
        }

        private static ItemTransaction GetStaticTestData()
        {
            return new ItemTransaction(GuidId, $"{GuidId}.txt", "CREATED");
        }
    }

    public class CloudTableMock : CloudTable
    {
        public CloudTableMock() : base(new Uri("http://127.0.0.1:10002/devstoreaccount1/"))
        {
        }
    }

}
