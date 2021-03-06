namespace Azure.Services.Interaction.Storage.Test.Helper
{
    using Azure.Services.Interaction.Storage.Test.Common;
    using Microsoft.Azure.Cosmos.Table;
    using System;
    /// <summary>
    /// Item Transaction class.
    /// </summary>
    public class ItemTransaction: TableEntity
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ItemTransaction()
        {
            ETag = "*";
            PartitionKey = Constants.PartitionKey;
            CreatedOn = DateTime.Now;
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        public ItemTransaction(string uuid, string name, string status) : this()
        {
            UUID = uuid;
            Name = name;
            RowKey = uuid;
            Status = status;
        }

        public string UUID { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
