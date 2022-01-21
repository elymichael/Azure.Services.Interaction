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
        /// Add Azure Storage Service Registration.
        /// </summary>
        /// <param name="services">Services Collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Services Collection.</returns>
        public static IServiceCollection AddAzureStorageServices<T>(this IServiceCollection services, IConfiguration configuration) where T: TableEntity
        {
            var config = configuration.GetSection("AzureServiceStorage");
            
            services.Configure<AzureTableStorageOptions>(config);

            services.AddSingleton(x => new BlobServiceClient(config.GetValue<string>("ConnectionString")));
            services.AddTransient<IAzureBlobStorage, AzureBlobStorageService>();
            services.AddTransient<IAzureTableStorage<T>, AzureTableStorageService<T>>();

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
