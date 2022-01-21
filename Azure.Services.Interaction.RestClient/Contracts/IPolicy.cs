using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.Services.Interaction.RestClient.Contracts
{
    /// <summary>
    /// Specifies the contract for a fault-handling policy.
    /// </summary>
    public interface IPolicy
    {
        /// <summary>
        /// Applies the configured policy to the execution of an action.
        /// </summary>
        /// <param name="action">An action to execute.</param>
        void Execute(Action action);

        /// <summary>
        /// Applies the configured policy to the execution of an action returning a value.
        /// </summary>
        /// <typeparam name="T">The type of the value being returned.</typeparam>
        /// <param name="action">An action to execute.</param>
        /// <returns>The return value the action.</returns>
        T Execute<T>(Func<T> action);

        /// <summary>
        /// Applies the configured policy to the execution of an async action.
        /// </summary>
        /// <param name="action">An async action to execute.</param>
        Task ExecuteAsync(Func<Task> action);

        /// <summary>
        /// Applies the configured policy to the execution of an async action returning a value.
        /// </summary>
        /// <typeparam name="T">The type of the value being returned.</typeparam>
        /// <param name="action">An async action to execute.</param>
        /// <returns>The return value the action.</returns>
        Task<T> ExecuteAsync<T>(Func<Task<T>> action);
    }
}