namespace Azure.Services.Interaction.RestClient.Contracts
{
    /// <summary>
    /// Specifies the contract for a fault-handling policy factory.
    /// The factory is responsible for creating different fault-handling strategies.
    /// </summary>
    public interface IPolicyFactory
    {
        /// <summary>
        /// Creates and returns a policy that allows retrying after waiting a fixed 
        /// amount of time.
        /// </summary>
        /// <param name="retryCount">Number of times the operation will be retried.</param>
        /// <param name="milliseconds">Time to wait in milliseconds between retries.</param>
        /// <returns>An instance of IPolicy configured with the specified parameters.</returns>
        IPolicy CreateConstantRetryPolicy(int retryCount, int milliseconds);

        /// <summary>
        /// Creates and returns a policy that implements an exponential back-off retry.
        /// </summary>
        /// <param name="retryCount">Number of times the operation will be retried.</param>
        /// <returns>An instance of IPolicy configured with the specified parameters.</returns>
        IPolicy CreateExponentialBackoffRetryPolicy(int retryCount);
    }
}
