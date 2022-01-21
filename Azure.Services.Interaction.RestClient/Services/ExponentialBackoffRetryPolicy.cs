namespace Azure.Services.Interaction.RestClient.Services
{
    using Azure.Services.Interaction.RestClient.Contracts;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Polly.Retry;
    using System;
    using System.Threading.Tasks;
    /// <summary>
    /// A Polly-based implementation of a base-2 exponential back-off retry policy.
    /// This policy will retry a failed action after increasing the wait time exponentially.
    /// Waiting time is 1, 2, 4, ..., 2^n seconds for the 1st, 2nd, 3rd and nth retry.
    /// </summary>
    public class ExponentialBackoffRetryPolicy : IPolicy
    {
        private readonly ILogger<ExponentialBackoffRetryPolicy> _logger;
        private readonly int _retryCount;

        /// <summary>
        /// Internal constructor for factory use.
        /// </summary>
        /// <param name="logger">An ILogger instance.</param>
        /// <param name="retryCount">Number of times the operation will be retried.</param>
        internal ExponentialBackoffRetryPolicy(ILogger<ExponentialBackoffRetryPolicy> logger, int retryCount)
        {
            _logger = logger;
            _retryCount = retryCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        public void Execute(Action action)
        {
            GetPolicy().Execute(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public T Execute<T>(Func<T> action)
        {
            return GetPolicy().Execute(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(Func<Task> action)
        {
            await GetAsyncPolicy().ExecuteAsync(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
        {
            return await GetAsyncPolicy().ExecuteAsync(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private AsyncRetryPolicy GetAsyncPolicy()
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetryAsync(
                            _retryCount,
                            attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)),
                            (exception, timeSpan, retryCount, context) =>
                                _logger.LogError($"Retrying after {timeSpan.TotalSeconds} seconds... attempt number {retryCount}"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private RetryPolicy GetPolicy()
        {
            return Policy.Handle<Exception>()
                         .WaitAndRetry(
                            _retryCount,
                            attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)),
                            (exception, timeSpan, retryCount, context) =>
                                _logger.LogError($"Retrying after {timeSpan.TotalSeconds} seconds... attempt number {retryCount}"));
        }
    }
}
