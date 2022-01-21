namespace Azure.Services.Interaction.API.Controllers
{
    using Azure.Services.Interaction.B2C.Commands;
    using Azure.Services.Interaction.B2C.Contracts;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class AzureServiceB2CController : ControllerBase
    {
        private readonly ILogger<AzureServiceB2CController> _logger;
        private readonly IAzureB2CUserService _azureB2CUserService;
        public AzureServiceB2CController(
            ILoggerFactory loggerFactory, 
            IAzureB2CUserService azureB2CUserService)
        {
            _logger = loggerFactory.CreateLogger<AzureServiceB2CController>();
            _azureB2CUserService = azureB2CUserService;
        }

        /// <summary>
        /// Get User Info.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpGet("GetUserInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserInfo(string emailAddress)
        {
            if (string.IsNullOrEmpty(emailAddress))
            {
                return BadRequest();
            }

            var result = await _azureB2CUserService.GetUserInfo(emailAddress);

            return Ok(result);
        }

        /// <summary>
        /// Get User Info.
        /// </summary>
        /// <returns>Status code</returns>
        [HttpGet("GetUserInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserInfo(DateTime filterDate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _azureB2CUserService.GetUsersActive(filterDate);

            return Ok(result);
        }

        [HttpPost("CreateUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser(CreateUserInfoCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _azureB2CUserService.AddUserInfo(command);

            return Ok(result);
        }

        [HttpPost("AddOrUpdateUserInfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SaveUserInfo(CreateUserInfoCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _azureB2CUserService.SaveUserInfo(command);

            return Ok(result);
        }

        [HttpPut("ChangePassword")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangePassword([FromBody]UserCredentialCommand command, string jobTitle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _azureB2CUserService.ChangePassword(command, jobTitle);

            return Ok();
        }

        [HttpPut("ChangeUserStatus")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeUserStatus([FromBody] UserStatusCommand command, string jobTitle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            await _azureB2CUserService.ChangeUserStatus(command, jobTitle);
            
            return Ok();
        }
    }
}
