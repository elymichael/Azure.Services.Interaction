namespace Azure.Services.Interaction.RestClient.Configuration
{
    /// <summary>
    /// Rest client options.
    /// </summary>
    public class RestClientOptions
    {
        public int TimeOut { get; set; }
        public int QueryRetries { get; set; }
    }
}
