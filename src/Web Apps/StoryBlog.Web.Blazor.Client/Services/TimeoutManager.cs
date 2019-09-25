using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace StoryBlog.Web.Client.Services
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TimeoutManager : ITimeoutManager, IDisposable
    {
        internal static readonly TimeSpan Never = TimeSpan.FromMilliseconds(-1.0d);

        private readonly Collection<TimeoutSubscription> subscriptions;
        private bool disposed;

        public TimeoutManager()
        {
            subscriptions = new Collection<TimeoutSubscription>();
        }

        /// <inheritdoc cref="ITimeoutManager.CreateTimeout" />
        public ITimeout CreateTimeout(TimeoutCallback callback, TimeSpan timeout)
        {
            EnsureNotDisposed();

            if (null == callback)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var subscription = new TimeoutSubscription(this, timeout, callback);

            subscriptions.Add(subscription);

            return subscription;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void RemoveSubscription(TimeoutSubscription subscription)
        {
            if (subscriptions.Remove(subscription))
            {
                ;
            }
        }

        private void Dispose(bool dispose)
        {
            if (disposed)
            {
                return;
            }

            try
            {
                if (dispose)
                {
                    var disposables = subscriptions.ToArray();
                    foreach (var disposable in disposables)
                    {
                        disposable.Dispose();
                    }
                }
            }
            finally
            {
                disposed = true;
            }
        }

        private void EnsureNotDisposed()
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(TimeoutManager));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class TimeoutSubscription : ITimeout
        {
            private readonly TimeoutManager manager;
            private readonly TimeoutCallback callback;
            private readonly Timer timer;
            private bool disposed;

            public TimeoutSubscription(TimeoutManager manager, TimeSpan timeout, TimeoutCallback callback)
            {
                this.manager = manager;
                this.callback = callback;

                timer = new Timer(OnTimerCallback, null, timeout, Never);
            }

            public void Dispose()
            {
                Dispose(true);
            }

            private void OnTimerCallback(object arg)
            {
                if (disposed)
                {
                    return;
                }

                callback.Invoke();

                Dispose();
            }

            private void Dispose(bool dispose)
            {
                if (disposed)
                {
                    return;
                }

                try
                {
                    if (dispose)
                    {
                        timer.Dispose();
                        manager.RemoveSubscription(this);
                    }
                }
                finally
                {
                    disposed = true;
                }
            }
        }
    }
}