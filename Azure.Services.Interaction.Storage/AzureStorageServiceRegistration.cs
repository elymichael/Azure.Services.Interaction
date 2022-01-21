namespace Azure.Services.Interaction.Storage
{
    using Azure.Storage.Blobs;
    using Azure.Services.Interaction.Storage.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Azure.Services.Interaction.Storage.Contracts;
    using Azure.Services.Interaction.Storage.Services;

    /// <summary>
    /// Azure Storage Service Registration.
    /// </summary>
    public static class AzureStorageServiceRegistration
    {
        /// <summary>
        /// Add Azure Storage Service Registration.
        /// </summary>
        /// <param name="services">Services Collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Services Collection.</returns>
        public static IServiceCollection AddAzureStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("AzureServiceStorage");
            
            services.Configure<AzureTableStorageOptions>(config);

            services.AddSingleton(x => new BlobServiceClient(config.GetValue<string>("ConnectionString")));
            services.AddTransient<IAzureBlobStorage, AzureBlobStorageService>();

            return services;
        }
    }
}
