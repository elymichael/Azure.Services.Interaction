namespace Azure.Services.Interaction.Storage.Services
{
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Azure.Services.Interaction.Storage.Contracts;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Azure blob storage service class.
    /// </summary>
    public class AzureBlobStorageService: IAzureBlobStorage
    {        
        /// <summary>
        /// Blob service client object.
        /// </summary>
        private readonly BlobServiceClient _blobServiceClient;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="blobServiceClient">Service client.</param>
        public AzureBlobStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        /// <summary>
        /// Get blob content.
        /// </summary>
        /// <param name="containerName">Container name.</param>
        /// <param name="name">File name.</param>
        /// <returns>Blob download result.</returns>
        public async Task<BlobDownloadResult> GetBlobAsync(string containerName, string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(name);
            if (blobClient == null) return null;
            var blobDownloadInfo = await blobClient.DownloadContentAsync();

            return blobDownloadInfo.Value;
        }

        /// <summary>
        /// Get all blob file names.
        /// </summary>
        /// <param name="containerName">Container name.</param>
        /// <param name="directoryPath">Specific folder to search list of files.</param>
        /// <returns>List of files.</returns>
        public List<string> GetAllBlobsNamesAsync(string containerName, string directoryPath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobs = containerClient.GetBlobs();
            
            return blobs
                .Where(x => x.Name.StartsWith(directoryPath))
                .Select(x => x.Name.Remove(0, directoryPath.Length + 1))
                .ToList();
        }

        /// <summary>
        /// Upload blob name.
        /// </summary>
        /// <param name="containerName">Container name.</param>
        /// <param name="name">File name.</param>
        /// <param name="file">File content.</param>
        /// <returns>No content</returns>
        public async Task UploadBlobAsync(string containerName, string name, Stream file)
        {
            await UploadBlobAsync(containerName, name, file, false);
        }

        /// <summary>
        /// Upload blob name.
        /// </summary>
        /// <param name="containerName">Container name.</param>
        /// <param name="name">File name.</param>
        /// <param name="file">File content.</param>
        /// <param name="overwrite"></param>
        /// <returns>No content</returns>
        public async Task UploadBlobAsync(string containerName, string name, Stream file, bool overwrite = false)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            var blobClient = containerClient.GetBlobClient(name);
            await blobClient.UploadAsync(file, overwrite);
        }

        /// <summary>
        /// Delete blog from a specific container name.
        /// </summary>
        /// <param name="containerName">Container name.</param>
        /// <param name="fileName">File name.</param>
        /// <returns>Return true if deleted.</returns>
        public async Task<bool> DeleteBlobAsync(string containerName, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            return await blobClient.DeleteIfExistsAsync();
        }
    }
}
