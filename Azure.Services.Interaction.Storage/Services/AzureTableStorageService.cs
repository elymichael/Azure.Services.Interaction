namespace Azure.Services.Interaction.Storage.Services
{
    using Azure.Services.Interaction.Storage.Configuration;
    using Azure.Services.Interaction.Storage.Contracts;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System.Threading.Tasks;

    /// <summary>
    /// Azure table storage service
    /// </summary>
    /// <typeparam name="T">Domain class type.</typeparam>
    public class AzureTableStorageService<T> : AzureTableStorageBase<T>, IAzureTableStorage<T> where T: TableEntity
    {
        /// <summary>
        /// Logger service.
        /// </summary>
        private readonly ILogger<AzureTableStorageService<T>> _logger;
        
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="loggerFactory">logger factory.</param>
        /// <param name="option">Azure storage options</param>
        public AzureTableStorageService(
            ILoggerFactory loggerFactory,
            IOptions<AzureTableStorageOptions> option) : base(option)
        {
            _logger = loggerFactory.CreateLogger<AzureTableStorageService<T>>();
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="entity">Table entity.</param>
        /// <returns>return domain class.</returns>
        public async Task<T> DeleteAsync(T entity)
        {
            var deleteOperation = TableOperation.Delete(entity);
            return await Execute(deleteOperation) as T;
        }

        /// <summary>
        /// Add or update entity.
        /// </summary>
        /// <param name="entity">Table entity.</param>
        /// <returns>Domain class.</returns>
        public async Task<T> AddOrUpdateAsync(T entity)
        {
            var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            return await Execute(insertOrMergeOperation) as T;
        }

        /// <summary>
        /// Get table entity.
        /// </summary>
        /// <param name="category">Partition key to filter data.</param>
        /// <param name="id">Identifier.</param>
        /// <returns>Domain class.</returns>
        public async Task<T> GetAsync(string category, string id)
        {
            var retrieveOperation = TableOperation.Retrieve<T>(category, id);
            return await Execute(retrieveOperation) as T;
        }
    }
}
