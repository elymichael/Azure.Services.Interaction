namespace Azure.Services.Interaction.ServiceBus.Test
{
    using Azure.Messaging.ServiceBus;
    using Azure.Services.Interaction.ServiceBus.Configuration;
    using Azure.Services.Interaction.ServiceBus.Model;
    using Azure.Services.Interaction.ServiceBus.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class AzureServiceBusServiceTest
    {
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private IOptions<ServiceBusOptions> _serviceBusOptions;

        private readonly Mock<ServiceBusClient> _serviceBusClientMock;
        private readonly Mock<ServiceBusSender> _serviceBusSenderMock;
        public AzureServiceBusServiceTest()
        {
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _serviceBusClientMock = new Mock<ServiceBusClient>();
            _serviceBusSenderMock = new Mock<ServiceBusSender>();

            _serviceBusClientMock.Setup(m => m.CreateSender(It.IsAny<string>())).Returns(_serviceBusSenderMock.Object);
            _serviceBusSenderMock.Setup(m => m.CreateMessageBatchAsync(It.IsAny<CancellationToken>())).Returns(null);
            _serviceBusSenderMock.Setup(m => m.SendMessagesAsync(It.IsAny<ServiceBusMessageBatch>(), It.IsAny<CancellationToken>()));
        }

        [Fact]
        public async void Send_Message_Queue_Successfully_Test()
        {
            _serviceBusOptions = Options.Create<ServiceBusOptions>(new ServiceBusOptions 
            { 
                QueueName = "main_queue", 
                ConnectionString = "Endpoint=sb://127.0.0.1:10005//;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=xxxxxxx",
                IsMock = true
            });

            var service = new AzureServiceBusService(_loggerFactoryMock.Object, _serviceBusOptions, _serviceBusClientMock.Object);
            await service.SendMessage(GetTestData());
            Assert.True(true);
        }

        [Fact]
        public async void Send_Message_Queue_Successfully_Fail_ConnectionString_Empty()
        {
            _serviceBusOptions = Options.Create<ServiceBusOptions>(new ServiceBusOptions());
                       
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await Task.Run(async () => {
                var service = new AzureServiceBusService(_loggerFactoryMock.Object, _serviceBusOptions, new ServiceBusClient(_serviceBusOptions.Value.ConnectionString));
                await service.SendMessage(GetTestData());
            }));
        }

        [Fact]
        public async void Send_Message_Queue_Successfully_Fail_Bad_ConnectionString()
        {
            _serviceBusOptions = Options.Create<ServiceBusOptions>(new ServiceBusOptions { QueueName = "", ConnectionString= "", IsMock = true });

            await Assert.ThrowsAsync<ArgumentException>(async () => await Task.Run(async () => {
                var service = new AzureServiceBusService(_loggerFactoryMock.Object, _serviceBusOptions, new ServiceBusClient(_serviceBusOptions.Value.ConnectionString));
                await service.SendMessage(GetTestData());
            }));
        }

        private static AzureServiceBusMessage GetTestData()
        {
            return new AzureServiceBusMessage
            {
                Level = "Low",
                Type = "Error",
                Service = "Azure.Services.Interaction.ServiceBus.Test",
                Message = "Test"
            };
        }
    }
}
