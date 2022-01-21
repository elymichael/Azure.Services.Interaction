namespace Azure.Services.Interaction.ServiceBus.Model
{
    /// <summary>
    /// Azure service bus message.
    /// </summary>
    public class AzureServiceBusMessage
    {
        /// <summary>
        /// The service name of the sender.
        /// </summary>
        public string Service { get; set; }
        /// <summary>
        /// Message information.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Additional Data associated to the Message.
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// Additional URL associated to the request.
        /// </summary>
        public string AdditionalUrl { get; set; }
        /// <summary>
        /// Type: Error, Info, Warning.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Level: High, Medium, Low.
        /// </summary>
        public string Level { get; set; }
    }
}
