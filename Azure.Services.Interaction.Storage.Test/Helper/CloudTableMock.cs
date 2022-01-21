namespace Azure.Services.Interaction.Storage.Test.Helper
{
    using Microsoft.Azure.Cosmos.Table;
    using System;

    /// <summary>
    /// Cloud Table class for mocking.
    /// </summary>
    public class CloudTableMock : CloudTable
    {
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public CloudTableMock() : base(new Uri("http://127.0.0.1:10002/devstoreaccount1/"))
        {
        }
    }
}
