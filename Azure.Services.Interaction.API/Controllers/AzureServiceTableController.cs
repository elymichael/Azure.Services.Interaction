namespace Azure.Services.Interaction.API.Controllers
{
    using Azure.Services.Interaction.API.Model;
    using Azure.Services.Interaction.Storage.Contracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class AzureServiceTableController : ControllerBase
    {
        private readonly ILogger<AzureServiceTableController> _logger;
        private readonly IAzureTableStorage<ItemTransaction> _azureTableStorage;
        public AzureServiceTableController(
            ILoggerFactory loggerFactory,
            IAzureTableStorage<ItemTransaction> azureTableStorage)
        {
            _logger = loggerFactory.CreateLogger<AzureServiceTableController>();
            _azureTableStorage = azureTableStorage;
        }

        /// <summary>
        /// Add or Update Item on Azure Table Storage.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPost("AddOrUpdateAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddOrUpdateAsync([FromBody] ItemTransaction command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _azureTableStorage.AddOrUpdateAsync(command);

            return Ok(result);
        }

        /// <summary>
        /// Delete Item from Azure Table Storage.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpDelete("DeleteAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteAsync([FromBody] ItemTransaction command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _azureTableStorage.DeleteAsync(command);

            return Ok(result);
        }

        /// <summary>
        /// Get Item from Azure Table Storage.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpGet("GetAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAsync(string category, string id)
        {
            if(string.IsNullOrEmpty(category) && string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var result = await _azureTableStorage.GetAsync(category, id);

            return Ok(result);
        }

    }
}
