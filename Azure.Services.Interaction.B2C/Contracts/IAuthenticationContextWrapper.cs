namespace Azure.Services.Interaction.B2C.Contracts
{
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public interface IAuthenticationContextWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<string> AcquireTokenAsync();
    }
}
