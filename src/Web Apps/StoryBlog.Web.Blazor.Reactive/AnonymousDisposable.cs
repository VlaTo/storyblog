using System;
using System.Threading;

namespace StoryBlog.Web.Blazor.Reactive
{
    /// <summary>
    /// Represents an Action-based disposable.
    /// </summary>
    internal sealed class AnonymousDisposable : ICancelable
    {
        private volatile Action dispose;

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => null == dispose;

        /// <summary>
        /// Constructs a new disposable with the given action used for disposal.
        /// </summary>
        /// <param name="dispose">Disposal action which will be run upon calling Dispose.</param>
        public AnonymousDisposable(Action dispose)
        {
            if (null == dispose)
            {
                throw new ArgumentNullException(nameof(dispose));
            }

            this.dispose = dispose;
        }

        /// <summary>
        /// Calls the disposal action if and only if the current instance hasn't been disposed yet.
        /// </summary>
        public void Dispose()
        {
            Interlocked.Exchange(ref dispose, null)?.Invoke();
        }
    }

    /// <summary>
    /// Represents a Action-based disposable that can hold onto some state.
    /// </summary>
    internal sealed class AnonymousDisposable<TState> : ICancelable
    {
        private TState state;
        private volatile Action<TState> dispose;

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => null == dispose;

        /// <summary>
        /// Constructs a new disposable with the given action used for disposal.
        /// </summary>
        /// <param name="state">The state to be passed to the disposal action.</param>
        /// <param name="dispose">Disposal action which will be run upon calling Dispose.</param>
        public AnonymousDisposable(TState state, Action<TState> dispose)
        {
            if(null == dispose)
            {
                throw new ArgumentNullException(nameof(dispose));
            }

            this.state = state;
            this.dispose = dispose;
        }

        /// <summary>
        /// Calls the disposal action if and only if the current instance hasn't been disposed yet.
        /// </summary>
        public void Dispose()
        {
            Interlocked.Exchange(ref dispose, null)?.Invoke(state);
            state = default;
        }
    }
}