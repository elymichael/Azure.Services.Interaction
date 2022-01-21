namespace Azure.Services.Interaction.ServiceBus.Configuration
{
    /// <summary>
    /// Service bus options.
    /// </summary>
    public class ServiceBusOptions
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
        public bool IsMock { get; set; }
     }
}
