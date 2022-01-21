namespace Azure.Services.Interaction.RestClient.Services
{
    using Azure.Services.Interaction.RestClient.Contracts;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;

    /// <summary>
    /// A Polly-based implementation of IPolicyFactory.
    /// </summary>
    public class PolicyFactory : IPolicyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public PolicyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retryCount"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public IPolicy CreateConstantRetryPolicy(int retryCount, int milliseconds)
        {
            if (retryCount < 1)
                throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "Value must be a positive integer");

            if (milliseconds < 1)
                throw new ArgumentOutOfRangeException(nameof(milliseconds), milliseconds, "Value must be a positive integer");

            var logger = _serviceProvider.GetRequiredService<ILogger<ConstantRetryPolicy>>();

            return new ConstantRetryPolicy(logger, retryCount, milliseconds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public IPolicy CreateExponentialBackoffRetryPolicy(int retryCount)
        {
            if (retryCount < 1)
                throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "Value must be a positive integer");

            var logger = _serviceProvider.GetRequiredService<ILogger<ExponentialBackoffRetryPolicy>>();

            return new ExponentialBackoffRetryPolicy(logger, retryCount);
        }
    }
}
