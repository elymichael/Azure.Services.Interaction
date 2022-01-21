namespace Azure.Services.Interaction.RestClient.Services
{
    using Azure.Services.Interaction.RestClient.Contracts;
    using Microsoft.Extensions.Logging;
    using Polly;
    using Polly.Retry;
    using System;
    using System.Threading.Tasks;
    /// <summary>
    /// A Polly-based implementation of a constant retry policy.
    /// This policy will retry a failed action after waiting for a fixed amount of time,
    /// as many times as indicated.
    /// </summary>
    public class ConstantRetryPolicy : IPolicy
    {
        private readonly ILogger<ConstantRetryPolicy> _logger;
        private readonly int _retryCount;
        private readonly int _milliseconds;

        /// <summary>
        /// Internal constructor for factory use.
        /// </summary>
        /// <param name="logger">An ILogger instance.</param>
        /// <param name="retryCount">Number of times the operation will be retried.</param>
        /// <param name="milliseconds">Time to wait in milliseconds between retries.</param>
        internal ConstantRetryPolicy(ILogger<ConstantRetryPolicy> logger, int retryCount, int milliseconds)
        {
            _logger = logger;
            _retryCount = retryCount;
            _milliseconds = milliseconds;
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
                            attempt => TimeSpan.FromMilliseconds(_milliseconds),
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
                            attempt => TimeSpan.FromMilliseconds(_milliseconds),
                            (exception, timeSpan, retryCount, context) =>
                                _logger.LogError($"Retrying after {timeSpan.TotalSeconds} seconds... attempt number {retryCount}"));
        }
    }
}
