using System;
using System.Collections.Generic;
using System.Threading;

namespace Metalbullz.IO
{
    /// <summary>
    /// Enumerator for the Pipeline.
    /// </summary>
    /// <typeparam name="T">The type of the data handled by the pipeline.</typeparam>
    public struct PipelineEnumerator<T> : IEnumerator<T>
    {
        private T _current;
        private readonly IPipeline<T> _pipeline;
        private readonly CancellationToken _token;

        internal PipelineEnumerator(IPipeline<T> pipeline, CancellationToken token)
        {
            _pipeline = pipeline;
            _token = token;
            _current = default!;
        }

        /// <inheritdoc />
        public T Current => _current;

        /// <inheritdoc />
        object System.Collections.IEnumerator.Current => Current!;

        /// <inheritdoc />
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public bool MoveNext()
        {
            try
            {
                _current = _pipeline.Receive(_token);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        /// <inheritdoc />
        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
