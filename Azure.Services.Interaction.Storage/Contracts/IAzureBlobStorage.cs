namespace Azure.Services.Interaction.Storage.Contracts
{
    using Azure.Storage.Blobs.Models;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Azure Blob Storage Interface
    /// </summary>
    public interface IAzureBlobStorage
    {
        /// <summary>
        /// Get Blob file.
        /// </summary>
        /// <param name="containerName">Container name.</param>
        /// <param name="blobName">File name.</param>
        /// <returns>Return Azure blob download result.</returns>
        Task<BlobDownloadResult> GetBlobAsync(string containerName, string blobName);
        /// <summary>
        /// Get file names in a specific directory.
        /// </summary>
        /// <param name="containerName">Container name.</param>
        /// <param name="directoryPath">Directory name.</param>
        /// <returns>List of file name.</returns>
        List<string> GetAllBlobsNamesAsync(string containerName, string directoryPath);
        /// <summary>
        /// Upload file to Azure blob storage.
        /// </summary>
        /// <param name="containerName">Azure container name.</param>
        /// <param name="blobName">File Name.</param>
        /// <param name="file">File Content (Memory Stream, FileStream).</param>
        /// <returns>N/A</returns>
        Task UploadBlobAsync(string containerName, string blobName, Stream file);
        /// <summary>
        /// Upload file to Azure blob storage.
        /// </summary>
        /// <param name="containerName">Azure container name.</param>
        /// <param name="name">File Name.</param>
        /// <param name="file">File Content (Memory Stream, FileStream).</param>
        /// <param name="overwrite">Overrite file if it is possible.</param>
        /// <returns>N/A</returns>
        Task UploadBlobAsync(string containerName, string name, Stream file, bool overwrite = false);
        /// <summary>
        /// Delete blob file using the filename.
        /// </summary>
        /// <param name="containerName">Azure container Name</param>
        /// <param name="fileName">File name</param>
        /// <returns>Return true</returns>
        Task<bool> DeleteBlobAsync(string containerName, string fileName);
    }
}
