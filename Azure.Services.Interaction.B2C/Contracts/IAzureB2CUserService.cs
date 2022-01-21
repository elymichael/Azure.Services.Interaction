namespace Azure.Services.Interaction.B2C.Contracts
{
    using Azure.Services.Interaction.B2C.Commands;
    using Azure.Services.Interaction.B2C.Model;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IAzureB2CUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task ChangePassword(UserCredentialCommand model, string customerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AzureB2CAdUserResult> AddUserInfo(CreateUserInfoCommand model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task ChangeUserStatus(UserStatusCommand model, string customerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<AzureB2CAdUserInfo> SaveUserInfo(CreateUserInfoCommand model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessEmailAddress"></param>
        /// <returns></returns>
        Task<AzureB2CAdUserSearchResult> GetUserInfo(string accessEmailAddress);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterDate"></param>
        /// <returns></returns>
        Task<List<AzureB2CAdUserSearchResult>> GetUsersActive(DateTime filterDate);
    }
}
