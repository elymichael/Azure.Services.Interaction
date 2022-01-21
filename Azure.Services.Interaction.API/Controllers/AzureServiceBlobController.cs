namespace Azure.Services.Interaction.API.Controllers
{
    using Azure.Services.Interaction.API.Model;
    using Azure.Services.Interaction.Storage.Contracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class AzureServiceBlobController : ControllerBase
    {
        private readonly ILogger<AzureServiceBlobController> _logger;
        private readonly IAzureBlobStorage _azureBlobStorage;
        public AzureServiceBlobController(
            ILoggerFactory loggerFactory, 
            IAzureBlobStorage azureBlobStorage)
        {
            _logger = loggerFactory.CreateLogger<AzureServiceBlobController>();
            _azureBlobStorage = azureBlobStorage;
        }

        /// <summary>
        /// Upload Item on Azure Blob Storage.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPut("UploadBlobAsync")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadBlobAsync([FromBody] BlobInformation command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var base64EncodedBytes = Convert.FromBase64String(command.Based64Content);
            using var ms = new MemoryStream();
            ms.Write(base64EncodedBytes, 0, base64EncodedBytes.Length);

            await _azureBlobStorage.UploadBlobAsync(command.ContainerName, command.BlobName, ms);

            return NoContent();
        }

        /// <summary>
        /// Delete Item from Azure Blob Storage.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpDelete("DeleteAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync(string blobName)
        {
            if (string.IsNullOrEmpty(blobName))
            {
                return BadRequest();
            }

            var result = await _azureBlobStorage.DeleteBlobAsync("", blobName);

            return Ok(result);
        }

        /// <summary>
        /// Get All blobs loaded to a specific directory.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpGet("GetBlobs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetBlobs(string containerName, string directoryName)
        {
            if (string.IsNullOrEmpty(containerName) && string.IsNullOrEmpty(directoryName))
            {
                return BadRequest();
            }

            var result = _azureBlobStorage.GetAllBlobsNamesAsync(containerName, directoryName);

            return Ok(result);
        }

        /// <summary>
        /// Get All blobs loaded to a specific directory.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpGet("GetBlob")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBlob(string containerName, string blobName)
        {
            if (string.IsNullOrEmpty(containerName) && string.IsNullOrEmpty(blobName))
            {
                return BadRequest();
            }

            var result = await _azureBlobStorage.GetBlobAsync(containerName, blobName);
            
            return Ok(result);
        }
    }
}
