using System.Collections.Generic;
using System.Threading;

namespace Metalbullz.IO
{
    /// <summary>
    /// Represents a pipeline for handling data.
    /// </summary>
    /// <typeparam name="T">The type of data handled by the pipeline.</typeparam>
    internal interface IPipeline<T>
    {
        /// <summary>
        /// Gets a value indicating whether the pipeline is closed.
        /// </summary>
        bool IsClosed { get; }

        /// <summary>
        /// Closes the pipeline.
        /// </summary>
        void Close();

        /// <summary>
        /// Receives an item from the pipeline.
        /// </summary>
        /// <returns>The received item.</returns>
        T Receive();

        /// <summary>
        /// Receives an item from the pipeline with the specified cancellation token.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The received item.</returns>
        T Receive(CancellationToken cancellationToken);

        /// <summary>
        /// Sends an item to the pipeline.
        /// </summary>
        /// <param name="item">The item to send.</param>
        void Send(T item);

        /// <summary>
        /// Sends an item to the pipeline with the specified cancellation token.
        /// </summary>
        /// <param name="item">The item to send.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        void Send(T item, CancellationToken cancellationToken);

        /// <summary>
        /// Yields items from the pipeline.
        /// </summary>
        /// <returns>An enumerable of items from the pipeline.</returns>
        IEnumerable<T> Yield();

        /// <summary>
        /// Yields items from the pipeline with the specified cancellation token.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An enumerable of items from the pipeline.</returns>
        IEnumerable<T> Yield(CancellationToken cancellationToken);
    }
}
