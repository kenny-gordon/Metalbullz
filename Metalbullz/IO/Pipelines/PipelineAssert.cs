using System;
using System.Threading;

namespace Metalbullz.IO
{
    /// <summary>
    /// Provides assertion methods for the Pipeline.
    /// </summary>
    internal static class PipelineAssert
    {
        /// <summary>
        /// Throws an <see cref="OperationCanceledException"/> if the cancellation token is canceled.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        internal static void IsOperationCancellationRequested(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException(cancellationToken);
        }

        /// <summary>
        /// Throws an <see cref="InvalidOperationException"/> if the pipeline is closed.
        /// </summary>
        /// <typeparam name="T">The type of the data handled by the pipeline.</typeparam>
        /// <param name="pipeline">The pipeline.</param>
        internal static void IsPipelineClosed<T>(IPipeline<T> pipeline)
        {
            if (pipeline.IsClosed)
                throw new InvalidOperationException("The pipeline is closed.");
        }
    }
}
