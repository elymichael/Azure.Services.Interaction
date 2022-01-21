namespace Azure.Services.Interaction.RestClient.Contracts
{
    using Azure.Services.Interaction.B2C.Helper;
    using System.Threading.Tasks;

    /// <summary>
    /// Rest client interface.
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Get objects of type T.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <returns>Get object.</returns>
        Task<RestResponse<T>> GetAsync<T>(string uri);

        /// <summary>
        /// Get objects of type T.
        /// </summary>
        /// <typeparam name="T">Type of the object to serialize.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <param name="accessToken">Access token</param>
        /// <returns>Get object.</returns>
        Task<RestResponse<T>> GetAsync<T>(string uri, string accessToken);

        /// <summary>
        /// Post objects of type T.
        /// </summary>
        /// <typeparam name="TIn">Type of the object to serialize.</typeparam>
        /// <typeparam name="TOut">Return object type.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <param name="input">Input object.</param>
        /// <returns></returns>
        Task<RestResponse<TOut>> PostAsync<TIn, TOut>(string uri, TIn input);

        /// <summary>
        /// Post objects of type T.
        /// </summary>
        /// <typeparam name="TIn">Type of the object to serialize.</typeparam>
        /// <typeparam name="TOut">Return object type.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="input">Input object.</param>
        /// <returns></returns>
        Task<RestResponse<TOut>> PostAsync<TIn, TOut>(string uri, string accessToken, TIn input);

        /// <summary>
        /// Put objects of type T.
        /// </summary>
        /// <typeparam name="TIn">Type of the object to serialize.</typeparam>
        /// <typeparam name="TOut">Return object type.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <param name="input">Input object.</param>
        /// <returns></returns>
        Task<RestResponse<TOut>> PutAsync<TIn, TOut>(string uri, TIn input);

        /// <summary>
        /// Put objects of type T.
        /// </summary>
        /// <typeparam name="TIn">Type of the object to serialize.</typeparam>
        /// <typeparam name="TOut">Return object type.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="input">Input object.</param>
        /// <returns></returns>
        Task<RestResponse<TOut>> PutAsync<TIn, TOut>(string uri, string accessToken, TIn input);

        /// <summary>
        /// Delete object.
        /// </summary>
        /// <typeparam name="T">Type of the object to delete.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <returns>Rest response.</returns>
        Task<RestResponse<T>> DeleteAsync<T>(string uri);
    }
}
