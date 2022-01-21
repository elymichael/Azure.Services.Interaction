namespace Azure.Services.Interaction.API.Controllers
{
    using Azure.Services.Interaction.ServiceBus.Contracts;
    using Azure.Services.Interaction.ServiceBus.Model;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class AzureServiceBusController : ControllerBase
    {
        private readonly ILogger<AzureServiceBusController> _logger;
        private readonly IAzureServiceBus _azureServiceBus;
        public AzureServiceBusController(
            ILoggerFactory loggerFactory, 
            IAzureServiceBus azureServiceBus)
        {
            _logger = loggerFactory.CreateLogger<AzureServiceBusController>();
            _azureServiceBus = azureServiceBus;
        }

        /// <summary>
        /// Send Json Message Bus Queue.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPut("SendJsonMessage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessage([FromBody] AzureServiceBusMessage command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _azureServiceBus.SendMessage(command);

            return NoContent();
        }

        /// <summary>
        /// Send Message Bus Queue.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpPut("SendMessage")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendMessage(string message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _azureServiceBus.SendMessage(message);

            return NoContent();
        }
    }
}
