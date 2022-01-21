namespace Azure.Services.Interaction.ServiceBus
{
    using Azure.Messaging.ServiceBus;
    using Azure.Services.Interaction.ServiceBus.Configuration;
    using Azure.Services.Interaction.ServiceBus.Contracts;
    using Azure.Services.Interaction.ServiceBus.Services;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// Azure service bus service registration.
    /// </summary>
    public static class AzureServiceBusServiceRegistration
    {
        /// <summary>
        /// Add Azure service bus service registration.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configuration">Configuration.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddAzureServiceBusServices(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("ServiceBus");

            services.Configure<ServiceBusOptions>(config);            
            services.AddSingleton(x => new ServiceBusClient(config.GetValue<string>("ConnectionString")));
            services.AddSingleton<IAzureServiceBus, AzureServiceBusService>();

            return services;
        }
    }
}
