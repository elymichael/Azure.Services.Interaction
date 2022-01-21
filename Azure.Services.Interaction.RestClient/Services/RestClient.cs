namespace Azure.Services.Interaction.RestClient.Services
{
    using Azure.Services.Interaction.B2C.Helper;
    using Azure.Services.Interaction.RestClient.Configuration;
    using Azure.Services.Interaction.RestClient.Contracts;
    using Azure.Services.Interaction.RestClient.Helper;
    using Azure.Services.Interaction.RestClient.Helper.Exceptions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;
    using System.Threading.Tasks;
    /// <summary>
    /// A GET HTTP REST client based on HttpClient.
    /// </summary>
    public class RestClient : IRestClient
    {
        private readonly IPolicyFactory _policyFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RestClient> _logger;
        private readonly RestClientOptions _options;

        /// <summary>
        /// Initializes an instance of RestRestClient.
        /// </summary>
        /// <param name="httpClientFactory">An HttpClientFactory instance.</param>
        /// <param name="logger">An ILogger instance.</param>
        /// <param name="options">A RestClient options.</param>
        public RestClient(IPolicyFactory policyFactory,
                               IHttpClientFactory httpClientFactory,
                               ILogger<RestClient> logger,
                               IOptions<RestClientOptions> options)
        {
            _policyFactory = policyFactory;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = options.Value;
        }

        /// <summary>
        /// Calls and retrieves the resource specified by an URI with an HTTP GET method.
        /// Expects a JSON in the response body.
        /// </summary>
        /// <typeparam name="T">The type of the resource.</typeparam>
        /// <param name="uri">The URI of the resource to query.</param>
        /// <returns>
        /// A QueryResponse&lt;T&gt; containing the resulting status code. If it is a 200
        /// an instance of T is also returned.
        /// </returns>
        public async Task<RestResponse<T>> GetAsync<T>(string uri)
        {
            try
            {
                IPolicy policy = _policyFactory.CreateExponentialBackoffRetryPolicy(_options.QueryRetries);

                using var response = await policy.ExecuteAsync(async () =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, uri);
                    request.Headers.Add("Accept", "application/json");

                    var httpClient = _httpClientFactory.CreateClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeOut);

                    return await httpClient.SendAsync(request);
                });

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    T content = await JsonSerializerHelper.DeserializeResponse<T>(response);
                    return new RestResponse<T>(response.StatusCode, content);
                }
                else
                {
                    return new RestResponse<T>(response.StatusCode);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new Helper.Exceptions.TimeOutException($"Timeout calling server at {uri}", ex);
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new DeserializationException($"Error deserializing JSON into type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Calls and retrieves the resource specified by an URI with an HTTP GET method.
        /// Expects a JSON in the response body.
        /// </summary>
        /// <typeparam name="T">The type of the resource.</typeparam>
        /// <param name="uri">The URI of the resource to query.</param>
        /// <param name="accessToken">Access token.</param>
        /// <returns>
        /// A QueryResponse&lt;T&gt; containing the resulting status code. If it is a 200
        /// an instance of T is also returned.
        /// </returns>
        public async Task<RestResponse<T>> GetAsync<T>(string uri, string accessToken)
        {
            try
            {
                IPolicy policy = _policyFactory.CreateExponentialBackoffRetryPolicy(_options.QueryRetries);

                using var response = await policy.ExecuteAsync(async () =>
                {
                    // Append the access token for the Graph API to the Authorization header of the request, using the Bearer scheme.
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, uri);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    var httpClient = _httpClientFactory.CreateClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeOut);

                    return await httpClient.SendAsync(request);
                });

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    T content = await JsonSerializerHelper.DeserializeResponse<T>(response);
                    return new RestResponse<T>(response.StatusCode, content);
                }
                else
                {
                    return new RestResponse<T>(response.StatusCode);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeOutException($"Timeout calling server at {uri}", ex);
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new DeserializationException($"Error deserializing JSON into type {typeof(T).Name}", ex);
            }
        }

        /// <summary>
        /// Post objects of type T.
        /// </summary>
        /// <typeparam name="TIn">Type of the object to serialize.</typeparam>
        /// <typeparam name="TOut">Return object type.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <param name="input">Input object.</param>
        /// <returns></returns>
        public async Task<RestResponse<TOut>> PostAsync<TIn, TOut>(string uri, TIn input)
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Add("Accept", "application/json");

                request.Content = JsonSerializerHelper.SerializeRequest(input);

                using (var httpClient = _httpClientFactory.CreateClient())
                {

                    httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeOut);
                    
                    var response = await httpClient.SendAsync(request);
                    
                    TOut content = await JsonSerializerHelper.DeserializeResponse<TOut>(response);
                    return new RestResponse<TOut>(response.StatusCode, content);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new Helper.Exceptions.TimeOutException($"Timeout calling server at {uri}", ex);
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new DeserializationException($"Error deserializing JSON into type {typeof(TOut).Name}", ex);
            }
        }

        /// <summary>
        /// Post objects of type T.
        /// </summary>
        /// <typeparam name="TIn">Type of the object to serialize.</typeparam>
        /// <typeparam name="TOut">Return object type.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="input">Input object.</param>
        /// <returns></returns>
        public async Task<RestResponse<TOut>> PostAsync<TIn, TOut>(string uri, string accessToken, TIn input)
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Add("Accept", "application/json");

                request.Content = JsonSerializerHelper.SerializeRequest(input);

                using (var httpClient = _httpClientFactory.CreateClient())
                {

                    httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeOut);
                    
                    var response = await httpClient.SendAsync(request);
                    
                    TOut content = await JsonSerializerHelper.DeserializeResponse<TOut>(response);
                    return new RestResponse<TOut>(response.StatusCode, content);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new Helper.Exceptions.TimeOutException($"Timeout calling server at {uri}", ex);
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new DeserializationException($"Error deserializing JSON into type {typeof(TOut).Name}", ex);
            }
        }

        /// <summary>
        /// Put objects of type T.
        /// </summary>
        /// <typeparam name="TIn">Type of the object to serialize.</typeparam>
        /// <typeparam name="TOut">Return object type.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <param name="input">Input object.</param>
        /// <returns></returns>
        public async Task<RestResponse<TOut>> PutAsync<TIn, TOut>(string uri, TIn input)
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Put, uri);
                request.Headers.Add("Accept", "application/json");

                request.Content = JsonSerializerHelper.SerializeRequest(input);

                using (var httpClient = _httpClientFactory.CreateClient())
                {

                    httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeOut);
                    
                    var response = await httpClient.SendAsync(request);
                    
                    TOut content = await JsonSerializerHelper.DeserializeResponse<TOut>(response);
                    return new RestResponse<TOut>(response.StatusCode, content);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new Helper.Exceptions.TimeOutException($"Timeout calling server at {uri}", ex);
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new DeserializationException($"Error deserializing JSON into type {typeof(TOut).Name}", ex);
            }
        }

        /// <summary>
        /// Put objects of type T.
        /// </summary>
        /// <typeparam name="TIn">Type of the object to serialize.</typeparam>
        /// <typeparam name="TOut">Return object type.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <param name="accessToken">Access token</param>
        /// <param name="input">Input object.</param>
        /// <returns></returns>
        public async Task<RestResponse<TOut>> PutAsync<TIn, TOut>(string uri, string accessToken, TIn input)
        {
            try
            {

                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Add("Accept", "application/json");

                request.Content = JsonSerializerHelper.SerializeRequest(input);

                using var httpClient = _httpClientFactory.CreateClient();

                httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeOut);
                var response = await httpClient.SendAsync(request);
                TOut content = await JsonSerializerHelper.DeserializeResponse<TOut>(response);
                return new RestResponse<TOut>(response.StatusCode, content);
            }
            catch (TaskCanceledException ex)
            {                
                throw new TimeOutException($"Timeout calling server at {uri}", ex);
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new DeserializationException($"Error deserializing JSON into type {typeof(TOut).Name}", ex);
            }
        }

        /// <summary>
        /// Delete object.
        /// </summary>
        /// <typeparam name="T">Type of the object to delete.</typeparam>
        /// <param name="uri">Url of the API endpoint.</param>
        /// <returns>Rest response.</returns>
        public async Task<RestResponse<T>> DeleteAsync<T>(string uri)
        {
            try
            {
                IPolicy policy = _policyFactory.CreateExponentialBackoffRetryPolicy(_options.QueryRetries);

                using var response = await policy.ExecuteAsync(async () =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Delete, uri);
                    request.Headers.Add("Accept", "application/json");

                    var httpClient = _httpClientFactory.CreateClient();
                    httpClient.Timeout = TimeSpan.FromSeconds(_options.TimeOut);

                    return await httpClient.SendAsync(request);
                });


                if (response.StatusCode == HttpStatusCode.OK)
                {
                    T content = await JsonSerializerHelper.DeserializeResponse<T>(response);
                    return new RestResponse<T>(response.StatusCode, content);
                }
                else
                {
                    return new RestResponse<T>(response.StatusCode);
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new Helper.Exceptions.TimeOutException($"Timeout calling server at {uri}", ex);
            }
            catch (HttpRequestException ex)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new DeserializationException($"Error deserializing JSON into type {typeof(T).Name}", ex);
            }
        }
    }
}
