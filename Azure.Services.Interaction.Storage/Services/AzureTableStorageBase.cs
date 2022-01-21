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
        private readonly CloudTable _cloudTable;
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="option">Azure storage options.</param>
        internal AzureTableStorageBase(IOptions<AzureTableStorageOptions> option, CloudTable cloudTable)
        {
            _azureTableStorageOptions = option.Value;
            _cloudTable = cloudTable;
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
                var tableResult = await _cloudTable.ExecuteAsync(tableOperation);
                return tableResult.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
