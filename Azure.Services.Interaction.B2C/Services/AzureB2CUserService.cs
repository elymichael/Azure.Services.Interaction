namespace Azure.Services.Interaction.B2C.Services
{
    using Azure.Services.Interaction.B2C.Commands;
    using Azure.Services.Interaction.B2C.Configuration;
    using Azure.Services.Interaction.B2C.Contracts;
    using Azure.Services.Interaction.B2C.Model;
    using Azure.Services.Interaction.RestClient.Contracts;
    using Azure.Services.Interaction.RestClient.Helper.Exceptions;
    using Microsoft.Extensions.Options;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class AzureB2CUserService : IAzureB2CUserService
    {
        private readonly AzureB2CClientOptions _azureB2CClientOptions;
        private readonly IRestClient _restClient;
        private readonly IAuthenticationContextWrapper _authenticationContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="azureB2CClientOptions"></param>
        /// <param name="restClient"></param>
        /// <param name="authenticationContext"></param>
        public AzureB2CUserService(
            IOptions<AzureB2CClientOptions> azureB2CClientOptions,
            IRestClient restClient,
            IAuthenticationContextWrapper authenticationContext)
        {
            _azureB2CClientOptions = azureB2CClientOptions.Value;
            _restClient = restClient;
            _authenticationContext = authenticationContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AzureB2CAdUserResult> AddUserInfo(CreateUserInfoCommand model)
        {
            var accessToken = await _authenticationContext.AcquireTokenAsync();

            string url = $"{_azureB2CClientOptions.GraphResourceUrl}{_azureB2CClientOptions.TenantId}/users?{_azureB2CClientOptions.GraphVersion}";

            var user = new CreateAdB2CUser
            {
                displayName = $"{model.Name} {model.LastName}",
                surname = model.LastName,
                jobTitle = model.CustomerId,
                signInNames = new Signinname[] { new Signinname { type = "emailAddress", value = model.EmailAddress } },
                userPrincipalName = $"{Guid.NewGuid()}@{_azureB2CClientOptions.Domain}",
                mailNickname = (model.EmailAddress).Split('@')[0],
                passwordProfile = new PasswordProfile
                {
                    forceChangePasswordNextLogin = false,
                    password = model.Password
                }
            };

            var response = await _restClient.PostAsync<CreateAdB2CUser, AzureB2CAdUserResult>(url, accessToken, user);
            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw new FailReturnCodeException($"Received status code {response.StatusCode} from Microsoft Tenant");
            }

            return response.Content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="jobTitle"></param>
        /// <returns></returns>
        public async Task ChangePassword(UserCredentialCommand model, string jobTitle)
        {
            var accessToken = await _authenticationContext.AcquireTokenAsync();
            // Getting User from Azure B2C-AD.
            var adUserInfo = await FilterUserInfo(accessToken, jobTitle);

            if (adUserInfo == null)
            {
                throw new EntityNotFoundExceptions("User not found");
            }

            // Prepare url for update
            string url = $"{_azureB2CClientOptions.GraphResourceUrl}{_azureB2CClientOptions.TenantId}/users/{adUserInfo.ObjectId}?{_azureB2CClientOptions.GraphVersion}";

            var user = new UserPasswordProfileCommand
            {
                passwordProfile = new PasswordProfile
                {
                    forceChangePasswordNextLogin = false,
                    password = model.NewPassword
                }
            };

            var response = await _restClient.PutAsync<UserPasswordProfileCommand, object>(url, accessToken, user);

            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new FailReturnCodeException($"Received status code {response.StatusCode} from Microsoft Tenant");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="jobTitle"></param>
        /// <returns></returns>
        public async Task ChangeUserStatus(UserStatusCommand model, string jobTitle)
        {
            // accountEnabled: true|false or userState
            var accessToken = await _authenticationContext.AcquireTokenAsync();
            // Getting User from Azure B2C-AD.
            var adUserInfo = await FilterUserInfo(accessToken, jobTitle);

            if (adUserInfo == null)
            {
                return;
            }

            if (adUserInfo.AccountEnabled == model.accountEnabled)
            {
                return;
            }
            // Prepare url for update
            string url = $"{_azureB2CClientOptions.GraphResourceUrl}{_azureB2CClientOptions.TenantId}/users/{adUserInfo.ObjectId}?{_azureB2CClientOptions.GraphVersion}";

            var response = await _restClient.PutAsync<UserStatusCommand, object>(url, accessToken, model);
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                throw new FailReturnCodeException($"Received status code {response.StatusCode} from Microsoft Tenant");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessEmailAddress"></param>
        /// <returns></returns>
        public async Task<AzureB2CAdUserSearchResult> GetUserInfo(string accessEmailAddress)
        {
            var accessToken = await _authenticationContext.AcquireTokenAsync();

            string url = $"{_azureB2CClientOptions.GraphResourceUrl}{_azureB2CClientOptions.TenantId}/users?{_azureB2CClientOptions.GraphVersion}&$filter=signInNames/any(x:x/value eq '{accessEmailAddress}')";

            var response = await _restClient.GetAsync<AzureB2CAdUserRoot>(url, accessToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new FailReturnCodeException($"Received status code {response.StatusCode} from Microsoft Tenant");
            }

            var user = response.Content.value
                .Select(s => new AzureB2CAdUserSearchResult
                {
                    ObjectId = s.objectId,
                    DisplayName = s.displayName,
                    AccountEnabled = s.accountEnabled,
                    jobTitle = s.jobTitle,
                    emailAddress = s.signInNames[0].value
                })
                .FirstOrDefault();

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterDate"></param>
        /// <returns></returns>
        public async Task<List<AzureB2CAdUserSearchResult>> GetUsersActive(DateTime filterDate)
        {
            var accessToken = await _authenticationContext.AcquireTokenAsync();

            string jsonDate = filterDate.ToString("yyyy-MM-ddTHH:mm:ss");

            string url = $"{_azureB2CClientOptions.GraphResourceUrl}{_azureB2CClientOptions.TenantId}/users?{_azureB2CClientOptions.GraphVersion}&$filter=accountEnabled eq true and createdDateTime le datetime'{jsonDate}'";

            var response = await _restClient.GetAsync<AzureB2CAdUserRoot>(url, accessToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new FailReturnCodeException($"Received status code {response.StatusCode} from Microsoft Tenant");
            }

            var users = response.Content.value
                .Where(w => w.jobTitle != null)
                .Select(s => new AzureB2CAdUserSearchResult
                {
                    ObjectId = s.objectId,
                    DisplayName = s.displayName,
                    AccountEnabled = s.accountEnabled,
                    jobTitle = s.jobTitle,
                    emailAddress = (s.signInNames?.Length > 0 ? s.signInNames[0].value : string.Empty)
                })
                .ToList();

            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<AzureB2CAdUserInfo> SaveUserInfo(CreateUserInfoCommand model)
        {
            // accountEnabled: true|false or userState
            var accessToken = await _authenticationContext.AcquireTokenAsync();

            // Getting User from Azure B2C-AD.
            var adUserInfo = await FilterUserInfo(accessToken, model.CustomerId);

            if (adUserInfo != null)
            {
                await ChangeUserStatus(new UserStatusCommand { accountEnabled = true }, model.CustomerId);
                adUserInfo.AccountEnabled = true;

                return adUserInfo;
            }

            if (adUserInfo == null)
            {
                var user = await GetUserInfo(model.EmailAddress);
                if (user != null)
                {
                    return new AzureB2CAdUserInfo
                    {
                        AccountEnabled = false,
                        DisplayName = user.DisplayName,
                        ObjectId = user.ObjectId
                    };
                }

            }

            var result = await this.AddUserInfo(model);

            return new AzureB2CAdUserInfo
            {
                AccountEnabled = true,
                DisplayName = result.displayName,
                ObjectId = result.objectId
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="jobTitle"></param>
        /// <returns></returns>
        private async Task<AzureB2CAdUserInfo> FilterUserInfo(string accessToken, string jobTitle)
        {
            string url = $"{_azureB2CClientOptions.GraphResourceUrl}{_azureB2CClientOptions.TenantId}/users?{_azureB2CClientOptions.GraphVersion}&$filter=jobTitle eq '{jobTitle}'";

            var response = await _restClient.GetAsync<AzureB2CAdUserRoot>(url, accessToken);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new FailReturnCodeException($"Received status code {response.StatusCode} from Microsoft Tenant");
            }

            var user = response.Content.value
                .Where(u => u.jobTitle == jobTitle)
                .Select(s => new AzureB2CAdUserInfo { ObjectId = s.objectId, DisplayName = s.displayName, AccountEnabled = s.accountEnabled })
                .FirstOrDefault();

            return user;
        }
    }
}
