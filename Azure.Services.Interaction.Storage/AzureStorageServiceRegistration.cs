namespace Azure.Services.Interaction.Storage
{
    using Azure.Storage.Blobs;
    using Azure.Services.Interaction.Storage.Configuration;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Azure.Services.Interaction.Storage.Contracts;
    using Azure.Services.Interaction.Storage.Services;
    using Microsoft.Azure.Cosmos.Table;

    /// <summary>
    /// Azure Storage Service Registration.
    /// </summary>
    public static class AzureStorageServiceRegistration
    {
        /// <summary>
        /// Add azure blob storage services.
        /// </summary>
        /// <param name="services">Service collection extension method.</param>
        /// <param name="configuration">Configuration parameter.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddAzureBlobStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("AzureServiceStorage");

            services.AddTransient<IAzureBlobStorage, AzureBlobStorageService>();
            services.AddSingleton(x => new BlobServiceClient(config.GetValue<string>("ConnectionString")));

            return services;
        }

        /// <summary>
        /// Add azure table storage services.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services">Service collection extension method.</param>
        /// <param name="configuration">Configuration parameter.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddAzureTableStorageServices<T>(this IServiceCollection services, IConfiguration configuration) where T : TableEntity
        {
            var config = configuration.GetSection("AzureServiceStorage");

            services.AddTransient<IAzureTableStorage<T>, AzureTableStorageService<T>>();
            services.Configure<AzureTableStorageOptions>(config);

            services.AddSingleton(provider =>
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(config.GetValue<string>("ConnectionString"));
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable table = tableClient.GetTableReference(typeof(T).Name);
                return table;
            });

            return services;
        }
    }
}
