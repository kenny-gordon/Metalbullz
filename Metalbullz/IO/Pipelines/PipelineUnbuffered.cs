using System;
using System.Collections.Generic;
using System.Threading;

#nullable enable

namespace Metalbullz.IO
{
    /// <summary>
    /// Represents an unbuffered pipeline for handling data of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data handled by the pipeline.</typeparam>
    internal class PipelineUnbuffered<T> : IPipeline<T>
    {
        private readonly Queue<PipelineHandler<T>> _queue;
        private readonly object _lockObject;

        /// <inheritdoc />
        public bool IsClosed { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineUnbuffered{T}"/> class.
        /// </summary>
        public PipelineUnbuffered()
        {
            _queue = new Queue<PipelineHandler<T>>();
            _lockObject = new object();
        }

        /// <inheritdoc />
        public void Close() => IsClosed = true;

        /// <inheritdoc />
        public T Receive() => Receive(CancellationToken.None);

        /// <inheritdoc />
        public T Receive(CancellationToken cancellationToken)
        {
            PipelineAssert.IsOperationCancellationRequested(cancellationToken);
            PipelineAssert.IsPipelineClosed(this);

            using (var receiver = new PipelineHandler<T>())
            {
                lock (_lockObject)
                {
                    _queue.Enqueue(receiver);
                }
                return receiver.WaitForValue(cancellationToken);
            }
        }

        /// <inheritdoc />
        public void Send(T item) => Send(item, CancellationToken.None);

        /// <inheritdoc />
        public void Send(T item, CancellationToken cancellationToken)
        {
            PipelineAssert.IsOperationCancellationRequested(cancellationToken);
            PipelineAssert.IsPipelineClosed(this);

            PipelineHandler<T>? handler = null;
            lock (_lockObject)
            {
                if (_queue.Count > 0)
                    handler = _queue.Dequeue();
            }

            if (handler != null)
                handler.Set(() => item);
            else
                throw new InvalidOperationException("There is no receiver available to receive the item.");
        }

        /// <inheritdoc />
        public IEnumerable<T> Yield() => Yield(CancellationToken.None);

        /// <inheritdoc />
        public IEnumerable<T> Yield(CancellationToken cancellationToken)
        {
            var enumerator = new PipelineEnumerator<T>(this, cancellationToken);
            while (enumerator.MoveNext())
                yield return enumerator.Current;
        }
    }
}
