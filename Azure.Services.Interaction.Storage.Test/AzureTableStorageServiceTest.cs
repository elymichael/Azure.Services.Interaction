namespace Azure.Services.Interaction.Storage.Test
{
    using Azure.Services.Interaction.Storage.Configuration;
    using Azure.Services.Interaction.Storage.Services;
    using Azure.Services.Interaction.Storage.Test.Common;
    using Azure.Services.Interaction.Storage.Test.Helper;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class AzureTableStorageServiceTest
    {
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private IOptions<AzureTableStorageOptions> _serviceTableStorageOptions;

        private const string GuidId = "988E4AF7-B42C-46E0-BDE4-6EBCE9A77DF8";

        public AzureTableStorageServiceTest()
        {
            _loggerFactoryMock = new Mock<ILoggerFactory>();

            _serviceTableStorageOptions = Options.Create<AzureTableStorageOptions>(new AzureTableStorageOptions
            {                
                ConnectionString = "http://mockuri"
            });
        }

        [Fact]
        public async void Loading_Constructor_Fail_ConnectionString_Empty()
        {
            var _localServiceTableStorageOptions = Options.Create<AzureTableStorageOptions>(new AzureTableStorageOptions());

            await Assert.ThrowsAsync<ArgumentNullException>(async () => await Task.Run(async () => {
                var service = new AzureTableStorageService<ItemTransaction>(_loggerFactoryMock.Object, _localServiceTableStorageOptions);
                await service.AddOrUpdateAsync(GetTestData());
            }));
        }

        [Fact]
        public async void Store_Table_Information_Successfully()
        {
            var service = new AzureTableStorageService<ItemTransaction>(_loggerFactoryMock.Object, _serviceTableStorageOptions);

            await service.AddOrUpdateAsync(GetStaticTestData());
            
            Assert.True(true);
        }

        [Fact]
        public async void Delete_Table_Information_Successfully()
        {
            var service = new AzureTableStorageService<ItemTransaction>(_loggerFactoryMock.Object, _serviceTableStorageOptions);
            
            await service.AddOrUpdateAsync(GetStaticTestData());

            await service.DeleteAsync(GetStaticTestData());

            Assert.True(true);
        }

        [Fact]
        public async void Get_Table_Information_Successfully()
        {
            var service = new AzureTableStorageService<ItemTransaction>(_loggerFactoryMock.Object, _serviceTableStorageOptions);

            await service.AddOrUpdateAsync(GetStaticTestData());

            var data = await service.GetAsync(Constants.PartitionKey, GuidId);

            Assert.NotNull(data);
        }

        private static ItemTransaction GetTestData()
        {
            return new ItemTransaction(Guid.NewGuid().ToString(), $"{Guid.NewGuid().ToString()}.txt", "CREATED");
        }

        private static ItemTransaction GetStaticTestData()
        {
            return new ItemTransaction(GuidId, $"{GuidId}.txt", "CREATED");
        }
    }
}
