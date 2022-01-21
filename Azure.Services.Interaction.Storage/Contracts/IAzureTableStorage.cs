namespace Azure.Services.Interaction.Storage.Contracts
{
    using Microsoft.Azure.Cosmos.Table;
    using System.Threading.Tasks;
    /// <summary>
    /// Azure table storage interface for T class of type TableEntity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAzureTableStorage<T> where T: TableEntity
    {
        /// <summary>
        /// Get entity using the category used in the PartitionKey.
        /// </summary>
        /// <param name="category">Partition Key</param>
        /// <param name="id">Identifier.</param>
        /// <returns>Return the Entity retrieved.</returns>
        Task<T> GetAsync(string category, string id);
        /// <summary>
        /// Add or update entity.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        /// <returns>Return the entity added or updated.</returns>
        Task<T> AddOrUpdateAsync(T entity);
        /// <summary>
        /// Delete an entity.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        /// <returns>Return the entity deleted.</returns>
        Task<T> DeleteAsync(T entity);
    }
}
