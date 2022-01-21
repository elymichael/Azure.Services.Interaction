namespace Azure.Services.Interaction.Storage.Services
{
    using Azure.Services.Interaction.Storage.Configuration;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Azure table storage base class.
    /// </summary>
    /// <typeparam name="T">Type of domain class.</typeparam>
    public class AzureTableStorageBase<T> where T: class
    {
        /// <summary>
        /// Storage options.
        /// </summary>
        private readonly AzureTableStorageOptions _azureTableStorageOptions;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="option">Azure storage options.</param>
        internal AzureTableStorageBase(IOptions<AzureTableStorageOptions> option)
        {
            _azureTableStorageOptions = option.Value;
        }
        /// <summary>
        /// Execute operation.
        /// </summary>
        /// <param name="tableOperation">Table operation.</param>
        /// <returns>return result of the operation.</returns>
        internal async Task<object> Execute(TableOperation tableOperation)
        {
            try
            {
                var table = await GetTable();
                var tableResult = await table.ExecuteAsync(tableOperation);
                return tableResult.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Retrieve the table.
        /// </summary>
        /// <returns>CloudTable.</returns>
        internal async Task<CloudTable> GetTable()
        {
            try
            {                
                var storageAccount = CloudStorageAccount.Parse(_azureTableStorageOptions.ConnectionString);
                var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
                var table = tableClient.GetTableReference(typeof(T).Name);
                await table.CreateIfNotExistsAsync();
                return table;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
