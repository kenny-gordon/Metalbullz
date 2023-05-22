using System;
using System.Collections.Generic;
using System.Threading;

namespace Metalbullz.IO
{
    /// <summary>
    /// Represents a buffered pipeline for handling data of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of data handled by the pipeline.</typeparam>
    internal class PipelineBuffered<T> : IPipeline<T>
    {
        private readonly AutoResetEvent _canReadEvent;
        private readonly AutoResetEvent _canSendEvent;
        private readonly int _capacity;
        private readonly Queue<T> _queue;
        private readonly object _lockObject;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineBuffered{T}"/> class with the specified capacity.
        /// </summary>
        /// <param name="capacity">The capacity of the pipeline buffer.</param>
        public PipelineBuffered(int capacity)
        {
            if (capacity < 1)
                throw new ArgumentOutOfRangeException(nameof(capacity), "The capacity must be greater than one.");

            _capacity = capacity;
            _queue = new Queue<T>();
            _canReadEvent = new AutoResetEvent(false);
            _canSendEvent = new AutoResetEvent(true); // Allow sending items initially
            _lockObject = new object();
        }

        /// <inheritdoc />
        public int Count => _queue.Count;

        /// <inheritdoc />
        public bool IsClosed { get; private set; }

        /// <inheritdoc />
        public void Close()
        {
            lock (_lockObject)
            {
                IsClosed = true;
                _canReadEvent.Set(); // Unblock any waiting receive calls
            }
        }

        /// <inheritdoc />
        public T Receive() => Receive(CancellationToken.None);

        /// <inheritdoc />
        public T Receive(CancellationToken cancellationToken)
        {
            while (true)
            {
                PipelineAssert.IsOperationCancellationRequested(cancellationToken);

                lock (_lockObject)
                {
                    if (_queue.Count > 0)
                    {
                        var item = _queue.Dequeue();
                        _canSendEvent.Set(); // Signal that a spot is available for sending
                        return item;
                    }

                    if (_queue.Count == 0)
                        PipelineAssert.IsPipelineClosed(this);

                    _canReadEvent.WaitOne();
                }
            }
        }

        /// <inheritdoc />
        public void Send(T item) => Send(item, CancellationToken.None);

        /// <inheritdoc />
        public void Send(T item, CancellationToken cancellationToken)
        {
            while (true)
            {
                PipelineAssert.IsOperationCancellationRequested(cancellationToken);

                lock (_lockObject)
                {
                    if (_queue.Count < _capacity)
                    {
                        _queue.Enqueue(item);
                        _canReadEvent.Set(); // Signal that an item is available for receiving
                        return;
                    }

                    PipelineAssert.IsPipelineClosed(this);
                    _canSendEvent.WaitOne();
                }
            }
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
