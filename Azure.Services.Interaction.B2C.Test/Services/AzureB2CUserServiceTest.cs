using Azure.Services.Interaction.B2C.Commands;
using Azure.Services.Interaction.B2C.Configuration;
using Azure.Services.Interaction.B2C.Contracts;
using Azure.Services.Interaction.B2C.Helper;
using Azure.Services.Interaction.B2C.Model;
using Azure.Services.Interaction.B2C.Services;
using Azure.Services.Interaction.B2C.Test.Helper;
using Azure.Services.Interaction.RestClient.Contracts;
using Azure.Services.Interaction.RestClient.Helper.Exceptions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Azure.Services.Interaction.B2C.Test.Services
{
    public class AzureB2CUserServiceTest
    {
        private readonly Mock<IOptions<AzureB2CClientOptions>> _azureB2CClientOptions;        
        private readonly Mock<IRestClient> _restClient;
        private readonly Mock<IAuthenticationContextWrapper> _authenticationContext;
        public AzureB2CUserServiceTest()
        {
            _restClient = new Mock<IRestClient>();
            _azureB2CClientOptions = new Mock<IOptions<AzureB2CClientOptions>>();
            _azureB2CClientOptions.Setup(m => m.Value).Returns(new AzureB2CClientOptions()
            {
                ClientId = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
                TenantId = "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
                ClientSecret = "xxXxwxxbxxxx_xxxxxxxx_~xxxxxx-xxxx",
                GraphResourceUrl = "https://graph.windows.net/",
                GraphVersion = "api-version=1.6",
                TenantLoginUrl = "https://login.microsoftonline.com/",
                AllowWebApiToBeAuthorizedByACL = true,
                Domain = "quilombo.onmicrosoft.com",
                Instance = "https://quilombo.b2clogin.com",
                SignUpSignInPolicyId = "B2C_1_JustSignIn"
            });

            _authenticationContext = new Mock<IAuthenticationContextWrapper>();
            _authenticationContext
                .Setup(m => m.AcquireTokenAsync())
                .Returns(Task.FromResult(It.IsAny<string>()));
        }

        [Fact]
        public async void ChangePassword_Ok()
        {
            var command = CommandsHelper.FakeCredentialCommand();

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PutAsync<UserPasswordProfileCommand, object>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserPasswordProfileCommand>()))
                .Returns(Task.FromResult(new RestResponse<object>(HttpStatusCode.NoContent, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await service.ChangePassword(command, "1");
            Assert.True(true);
        }

        [Fact]
        public async void ChangePassword_Throw_EntityNotFoundExceptions()
        {
            var command = CommandsHelper.FakeCredentialCommand();

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PutAsync<UserPasswordProfileCommand, object>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserPasswordProfileCommand>()))
                .Returns(Task.FromResult(new RestResponse<object>(HttpStatusCode.NoContent, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await Assert.ThrowsAsync<EntityNotFoundExceptions>(async () => await service.ChangePassword(command, "5"));
        }

        [Fact]
        public async void ChangePassword_Throw_FailReturnCodeException_In_Get()
        {
            var command = CommandsHelper.FakeCredentialCommand();

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.BadRequest, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PutAsync<UserPasswordProfileCommand, object>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserPasswordProfileCommand>()))
                .Returns(Task.FromResult(new RestResponse<object>(HttpStatusCode.NoContent, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await Assert.ThrowsAsync<FailReturnCodeException>(async () => await service.ChangePassword(command, "1"));
        }

        [Fact]
        public async void ChangePassword_Throw_FailReturnCodeException_In_Put()
        {
            var command = CommandsHelper.FakeCredentialCommand();

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PutAsync<UserPasswordProfileCommand, object>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserPasswordProfileCommand>()))
                .Returns(Task.FromResult(new RestResponse<object>(HttpStatusCode.BadRequest, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object,  _restClient.Object, _authenticationContext.Object);

            await Assert.ThrowsAsync<FailReturnCodeException>(async () => await service.ChangePassword(command, "1"));
        }

        [Fact]
        public async void AddUserInfo_Ok()
        {
            var command = CommandsHelper.FakeAddUserInfoCommand();

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PostAsync<CreateAdB2CUser, AzureB2CAdUserResult>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CreateAdB2CUser>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserResult>(HttpStatusCode.Created, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await service.AddUserInfo(command);
            Assert.True(true);
        }

        [Fact]
        public async void AddUserInfo_Throw_FailReturnCodeException_In_Post()
        {
            var command = CommandsHelper.FakeAddUserInfoCommand();

            _restClient
                .Setup(m => m.PostAsync<CreateAdB2CUser, AzureB2CAdUserResult>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CreateAdB2CUser>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserResult>(HttpStatusCode.BadRequest, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await Assert.ThrowsAsync<FailReturnCodeException>(async () => await service.AddUserInfo(command));
        }

        [Fact]
        public async void ChangeUserStatus_Ok()
        {
            var command = new UserStatusCommand() { accountEnabled = true };

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PutAsync<UserStatusCommand, object>(It.IsAny<string>(), It.IsAny<string>(), command))
                .Returns(Task.FromResult(new RestResponse<object>(HttpStatusCode.NoContent, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await service.ChangeUserStatus(command, "1");
            Assert.True(true);
        }

        [Fact]
        public async void ChangeUserStatus_Throw_FailReturnCodeException_In_Get()
        {
            var command = new UserStatusCommand() { accountEnabled = true };

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.BadRequest, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PutAsync<UserStatusCommand, object>(It.IsAny<string>(), It.IsAny<string>(), command))
                .Returns(Task.FromResult(new RestResponse<object>(HttpStatusCode.NoContent, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await Assert.ThrowsAsync<FailReturnCodeException>(async () => await service.ChangeUserStatus(command, "1"));
        }

        [Fact]
        public async void ChangeUserStatus_Throw_FailReturnCodeException_In_Put()
        {
            var command = new UserStatusCommand() { accountEnabled = true };

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PutAsync<UserStatusCommand, object>(It.IsAny<string>(), It.IsAny<string>(), command))
                .Returns(Task.FromResult(new RestResponse<object>(HttpStatusCode.BadRequest, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await Assert.ThrowsAsync<FailReturnCodeException>(async () => await service.ChangeUserStatus(command, "1"));
        }


        [Fact]
        public async void SaveUserInfo_Ok()
        {
            var command = CommandsHelper.FakeAddUserInfoCommand();

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PutAsync<UserStatusCommand, object>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserStatusCommand>()))
                .Returns(Task.FromResult(new RestResponse<object>(HttpStatusCode.NoContent, null)));

            _restClient
                .Setup(m => m.PostAsync<CreateAdB2CUser, AzureB2CAdUserResult>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CreateAdB2CUser>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserResult>(HttpStatusCode.Created, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await service.AddUserInfo(command);
            Assert.True(true);
        }

        [Fact]
        public async void SaveUserInfo_Throw_FailReturnCodeException_In_Post()
        {
            var command = CommandsHelper.FakeAddUserInfoCommand();

            _restClient
               .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            _restClient
                .Setup(m => m.PutAsync<UserStatusCommand, object>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserStatusCommand>()))
                .Returns(Task.FromResult(new RestResponse<object>(HttpStatusCode.NoContent, null)));

            _restClient
                .Setup(m => m.PostAsync<CreateAdB2CUser, AzureB2CAdUserResult>(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CreateAdB2CUser>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserResult>(HttpStatusCode.BadRequest, null)));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await Assert.ThrowsAsync<FailReturnCodeException>(async () => await service.AddUserInfo(command));
        }

        [Fact]
        public async void GetUserInfo_ok()
        {
            _restClient
               .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            var user = await service.GetUserInfo("ely.nunez@truenorth.co");

            Assert.NotNull(user);
        }


        [Fact]
        public async void GetUserInfo_Throw_FailReturnCodeException_In_Get()
        {
            var command = new UserStatusCommand() { accountEnabled = true };

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.BadRequest, CommandsHelper.GetB2CUserMockData())));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await Assert.ThrowsAsync<FailReturnCodeException>(async () => await service.GetUserInfo("ely.nunez@truenorth.co"));
        }

        [Fact]
        public async void GetUsersActive_ok()
        {
            _restClient
               .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.OK, CommandsHelper.GetB2CUserMockData())));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            var users = await service.GetUsersActive(System.DateTime.Now);

            Assert.NotNull(users);
        }

        [Fact]
        public async void GetUsersActive_Throw_FailReturnCodeException_In_Get()
        {
            var command = new UserStatusCommand() { accountEnabled = true };

            _restClient
                .Setup(m => m.GetAsync<AzureB2CAdUserRoot>(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new RestResponse<AzureB2CAdUserRoot>(HttpStatusCode.BadRequest, CommandsHelper.GetB2CUserMockData())));

            var service = new AzureB2CUserService(_azureB2CClientOptions.Object, _restClient.Object, _authenticationContext.Object);

            await Assert.ThrowsAsync<FailReturnCodeException>(async () => await service.GetUsersActive(System.DateTime.Now));
        }
    }
}
