namespace Azure.Services.Interaction.B2C
{
    using Azure.Services.Interaction.B2C.Configuration;
    using Azure.Services.Interaction.B2C.Contracts;
    using Azure.Services.Interaction.B2C.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Class Azure B2C Service Collection Registration
    /// </summary>
    public static class AzureB2CServiceRegistration
    {
        /// <summary>
        /// Add Azure B2C Service Collection.
        /// Add variable 'AzureB2CClient' in the AppSettings.json for B2C support.
        /// </summary>
        /// <param name="services">Services Collection</param>
        /// <param name="configuration">Configuration</param>
        /// <returns>Service Collection</returns>
        public static IServiceCollection AddAzureB2CServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddTransient<IAuthenticationContextWrapper, AuthenticationContextWrapper>()
                .AddTransient<IAzureB2CUserService, AzureB2CUserService>();

            services.Configure<AzureB2CClientOptions>(configuration.GetSection("AzureB2CClient"));

            return services;
        }
    }
}
