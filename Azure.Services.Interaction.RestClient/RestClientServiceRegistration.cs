namespace Azure.Services.Interaction.RestClient
{
    using Azure.Services.Interaction.RestClient.Contracts;
    using Azure.Services.Interaction.RestClient.Services;
    using Microsoft.Extensions.DependencyInjection;

    public static class RestClientServiceRegistration
    {
        /// <summary>
        /// IServiceCollection extension method to register a policy factory for Polly-based policies.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <returns>The original services object.</returns>
        public static IServiceCollection AddHttpClientAndPolicyFactory(this IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddTransient<IRestClient, RestClient>();
            services.AddTransient<IPolicyFactory, PolicyFactory>();

            return services;
        }
    }
}
