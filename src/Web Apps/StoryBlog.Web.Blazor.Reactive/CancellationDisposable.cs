using System;
using System.Threading;

namespace StoryBlog.Web.Blazor.Reactive
{
    /// <summary>
    /// Represents a disposable resource that has an associated <seealso cref="CancellationToken"/> that will be set to the cancellation requested state upon disposal.
    /// </summary>
    public sealed class CancellationDisposable : ICancelable
    {
        private readonly CancellationTokenSource cts;

        /// <summary>
        /// Gets the <see cref="CancellationToken"/> used by this <see cref="CancellationDisposable"/>.
        /// </summary>
        public CancellationToken Token => cts.Token;

        /// <summary>
        /// Gets a value that indicates whether the object is disposed.
        /// </summary>
        public bool IsDisposed => cts.IsCancellationRequested;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationDisposable"/> class that uses an existing <seealso cref="CancellationTokenSource"/>.
        /// </summary>
        /// <param name="cts"><seealso cref="CancellationTokenSource"/> used for cancellation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="cts"/> is <c>null</c>.</exception>
        public CancellationDisposable(CancellationTokenSource cts)
        {
            if (null == cts)
            {
                throw new ArgumentNullException(nameof(cts));
            }

            this.cts = cts;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancellationDisposable"/> class that uses a new <seealso cref="CancellationTokenSource"/>.
        /// </summary>
        public CancellationDisposable()
            : this(new CancellationTokenSource())
        {
        }

        /// <summary>
        /// Cancels the underlying <seealso cref="CancellationTokenSource"/>.
        /// </summary>
        public void Dispose() => cts.Cancel();
    }
}