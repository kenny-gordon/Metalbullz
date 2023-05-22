using System.Collections.Generic;
using System.Threading;

namespace Metalbullz.IO
{
    /// <summary>
    /// Represents a pipeline for handling data of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data handled by the pipeline.</typeparam>
    public class Pipeline<T> : IPipeline<T>
    {
        private readonly IPipeline<T> _pipeline;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pipeline{T}"/> class.
        /// </summary>
        /// <param name="capacity">The optional capacity of the pipeline buffer.</param>
        public Pipeline(int? capacity = null)
        {
            _pipeline = capacity.HasValue
                ? new PipelineBuffered<T>(capacity.Value)
                : new PipelineUnbuffered<T>();
        }

        /// <inheritdoc />
        public bool IsClosed => _pipeline.IsClosed;

        /// <inheritdoc />
        public void Close() => _pipeline.Close();

        /// <inheritdoc />
        public T Receive() => _pipeline.Receive();

        /// <inheritdoc />
        public T Receive(CancellationToken cancellationToken) => _pipeline.Receive(cancellationToken);

        /// <inheritdoc />
        public void Send(T item) => _pipeline.Send(item);

        /// <inheritdoc />
        public void Send(T item, CancellationToken cancellationToken) => _pipeline.Send(item, cancellationToken);

        /// <inheritdoc />
        public IEnumerable<T> Yield() => _pipeline.Yield();

        /// <inheritdoc />
        public IEnumerable<T> Yield(CancellationToken cancellationToken) => _pipeline.Yield(cancellationToken);
    }
}
