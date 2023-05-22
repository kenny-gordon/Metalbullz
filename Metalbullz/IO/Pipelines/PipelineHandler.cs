using System;
using System.Threading;
#nullable enable

namespace Metalbullz.IO
{
    /// <summary>
    /// Represents a handler for the Pipeline.
    /// </summary>
    /// <typeparam name="T">The type of the data handled by the Pipeline.</typeparam>
    public class PipelineHandler<T> : IDisposable
    {
        private ManualResetEventSlim _eventHandle;
        private Func<T>? _valueFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="PipelineHandler{T}"/> class.
        /// </summary>
        internal PipelineHandler()
        {
            _eventHandle = new ManualResetEventSlim(false);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="PipelineHandler{T}"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets the function to be executed when the value is requested.
        /// </summary>
        /// <param name="func">The function that provides the value.</param>
        public void Set(Func<T> func)
        {
            if (_valueFunc == null)
                throw new ArgumentNullException(nameof(func));

            _valueFunc = func;
            _eventHandle.Set();
        }

        /// <summary>
        /// Waits for the value to be available and returns it.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The value.</returns>
        internal T WaitForValue(CancellationToken cancellationToken)
        {
            while (_valueFunc == null)
            {
                PipelineAssert.IsOperationCancellationRequested(cancellationToken);
                _eventHandle.Wait(cancellationToken);
            }

            return _valueFunc();
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="PipelineHandler{T}"/> object
        /// and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_eventHandle != null)
                {
                    _eventHandle.Dispose();
                    _eventHandle = null!;
                }
            }
        }
    }
}
