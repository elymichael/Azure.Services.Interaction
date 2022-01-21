namespace Azure.Services.Interaction.ServiceBus.Services
{
    using Azure.Messaging.ServiceBus;
    using Azure.Services.Interaction.ServiceBus.Configuration;
    using Azure.Services.Interaction.ServiceBus.Contracts;
    using Azure.Services.Interaction.ServiceBus.Model;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Threading.Tasks;

    public class AzureServiceBusService : IAzureServiceBus
    {
        private readonly ILogger<AzureServiceBusService> _logger;

        private readonly ServiceBusOptions _serviceBusOption;
        private readonly ServiceBusClient _client;
        private ServiceBusSender _sender;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="loggerFactory">logger factory.</param>
        /// <param name="option">Azure service bus options.</param>
        /// <param name="client">Service bus client.</param>
        public AzureServiceBusService(
            ILoggerFactory loggerFactory,
            IOptions<ServiceBusOptions> option,
            ServiceBusClient client)
        {
            _logger = loggerFactory.CreateLogger<AzureServiceBusService>();

            _serviceBusOption = option.Value;
            _client = client;
            _sender = _client.CreateSender(_serviceBusOption.QueueName);
        }

        /// <summary>
        /// Send message.
        /// </summary>
        /// <param name="message">message string.</param>
        /// <returns>N/A</returns>
        public async Task SendMessage(string message)
        {
            try
            {
                using ServiceBusMessageBatch messageBatch = await _sender.CreateMessageBatchAsync();
                if (!_serviceBusOption.IsMock)
                {
                    if (!messageBatch.TryAddMessage(new ServiceBusMessage(message)))
                    {
                        throw new Exception($"The message is too large to fit in the batch.");
                    }
                }

                await _sender.SendMessagesAsync(messageBatch);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message, message);
            }
        }

        /// <summary>
        /// Send message.
        /// </summary>
        /// <param name="message">Message content.</param>
        /// <returns>N/A</returns>
        public async Task SendMessage(AzureServiceBusMessage message)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            string messageInfo = JsonConvert.SerializeObject(message, serializerSettings);

            await SendMessage(messageInfo);
        }
    }
}
