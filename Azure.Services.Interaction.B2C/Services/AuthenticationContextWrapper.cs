namespace Azure.Services.Interaction.B2C.Services
{
    using Azure.Services.Interaction.B2C.Configuration;
    using Azure.Services.Interaction.B2C.Contracts;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationContextWrapper : IAuthenticationContextWrapper
    {
        private readonly AzureB2CClientOptions _azureB2CClientOptions;
        private AuthenticationContext authContext;
        private ClientCredential credential;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="options"></param>
        public AuthenticationContextWrapper(IOptions<AzureB2CClientOptions> options)
        {
            _azureB2CClientOptions = options.Value;
            authContext = new AuthenticationContext($"{_azureB2CClientOptions.TenantLoginUrl}{_azureB2CClientOptions.TenantId}");
            credential = new ClientCredential(_azureB2CClientOptions.ClientId, _azureB2CClientOptions.ClientSecret);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> AcquireTokenAsync()
        {
            // Getting access token from Azure.
            var authData = await authContext.AcquireTokenAsync(_azureB2CClientOptions.GraphResourceUrl, credential);
            return authData.AccessToken;
        }
    }
}
