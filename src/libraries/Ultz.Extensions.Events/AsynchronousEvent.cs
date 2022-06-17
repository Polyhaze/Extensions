using System;
using System.Threading.Tasks;

namespace Ultz.Extensions.Events
{
    /// <summary>
    /// Represents an asynchronous event handler caller.
    /// </summary>
    /// <typeparam name="T"> The <see cref="Type" /> of <see cref="EventArgs" /> used by this event. </typeparam>
    public sealed class AsynchronousEvent<T> where T : EventArgs
    {
        private readonly Func<Exception, Task> _errorHandler;

        private readonly object _lock = new object();

        /// <summary>
        /// Initialises a new <see cref="AsynchronousEvent{T}" />.
        /// </summary>
        public AsynchronousEvent()
        {
        }

        /// <summary>
        /// Initialises a new <see cref="AsynchronousEvent{T}" /> with the specified <see cref="Func{T, TResult}" /> error handler.
        /// </summary>
        /// <param name="errorHandler"> The error handler for exceptions occurring in event handlers. </param>
        public AsynchronousEvent(Func<Exception, Task> errorHandler)
        {
            _errorHandler = errorHandler;
        }

        private event AsynchronousEventHandler<T> Delegate;

        /// <summary>
        /// Hooks an <see cref="AsynchronousEventHandler{T}" /> up to this <see cref="AsynchronousEvent{T}" />.
        /// </summary>
        /// <param name="handler"> The <see cref="AsynchronousEventHandler{T}" /> to hook up. </param>
        public void Hook(AsynchronousEventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            lock (_lock)
            {
                Delegate += handler;
            }
        }

        /// <summary>
        /// Unhooks an <see cref="AsynchronousEventHandler{T}" /> from this <see cref="AsynchronousEvent{T}" />.
        /// </summary>
        /// <param name="handler"> The <see cref="AsynchronousEventHandler{T}" /> to unhook. </param>
        public void Unhook(AsynchronousEventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            lock (_lock)
            {
                Delegate -= handler;
            }
        }

        /// <summary>
        /// Unhooks all <see cref="AsynchronousEventHandler{T}" />s from this <see cref="AsynchronousEvent{T}" />.
        /// </summary>
        public void UnhookAll()
        {
            lock (_lock)
            {
                Delegate = null;
            }
        }

        /// <summary>
        /// Invokes this <see cref="AsynchronousEventHandler{T}" />, sequentially invoking each hooked up
        /// <see cref="AsynchronousEventHandler{T}" />.
        /// </summary>
        /// <param name="e"> The <see cref="EventArgs" /> data for this invocation. </param>
        public async Task InvokeAsync(T e)
        {
            Delegate[] list;
            lock (_lock)
            {
                list = Delegate?.GetInvocationList();
            }

            if (list == null)
            {
                return;
            }

            for (var i = 0; i < list.Length; i++)
            {
                var task = ((AsynchronousEventHandler<T>) list[i])(e);
                if (task == null)
                {
                    continue;
                }

                try
                {
                    await task.ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    if (_errorHandler != null)
                    {
                        await _errorHandler(ex).ConfigureAwait(false);
                    }
                }
            }
        }
    }
}