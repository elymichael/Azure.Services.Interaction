namespace Azure.Services.Interaction.ServiceBus.Contracts
{
    using Azure.Services.Interaction.ServiceBus.Model;
    using System.Threading.Tasks;

    /// <summary>
    /// Azure service bus class.
    /// </summary>
    public interface IAzureServiceBus
    {
        /// <summary>
        /// Send message.
        /// </summary>
        /// <param name="message">message string.</param>
        /// <returns>N/A</returns>
        Task SendMessage(string message);

        /// <summary>
        /// Send message.
        /// </summary>
        /// <param name="message">Message content.</param>
        /// <returns>N/A</returns>
        Task SendMessage(AzureServiceBusMessage message);
    }
}
