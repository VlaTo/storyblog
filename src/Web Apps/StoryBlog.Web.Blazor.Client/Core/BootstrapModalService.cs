using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Blazor.Client.Components;
using StoryBlog.Web.Blazor.Client.Extensions;

namespace StoryBlog.Web.Blazor.Client.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BootstrapModalService : IModalService
    {
        private readonly IDictionary<Type, List<ModalSubscription>> subscriptions;

        /// <summary>
        /// 
        /// </summary>
        public BootstrapModalService()
        {
            subscriptions = new Dictionary<Type, List<ModalSubscription>>();
        }

        /// <inheritdoc cref="IModalService.ShowAsync" />
        public Task<ModalButton> ShowAsync(IModalContent modalContent, CancellationToken cancellationToken = default)
        {
            if (null == modalContent)
            {
                throw new ArgumentNullException(nameof(modalContent));
            }

            if (subscriptions.TryGetValue(modalContent.GetType(), out var observers))
            {
                var tcs = new TaskCompletionSource<ModalButton>();

                foreach (var subscription in observers)
                {
                    subscription.Observer.ShowContent(modalContent, tcs, cancellationToken);
                }

                //return cancellationToken == default ? tcs.Task : tcs.AsTask(cancellationToken);

                return tcs.Task;
            }

            return Task.FromResult<ModalButton>(null);
        }

        /// <inheritdoc cref="IModalService.Subscribe{T}" />
        public IDisposable Subscribe<T>(IModalContentObserver observer) where T : IModalContent
        {
            var key = typeof(T);

            if (false == subscriptions.TryGetValue(key, out var observers))
            {
                observers = new List<ModalSubscription>();
                subscriptions.Add(key, observers);
            }

            var subscription = new ModalSubscription(this, observer);

            observers.Add(subscription);

            return subscription;
        }

        private void ReleaseObserver(IModalContentObserver observer)
        {
            foreach (var kvp in subscriptions)
            {
                var observers = kvp.Value;
                var index = observers.FindIndex(subscription => ReferenceEquals(subscription.Observer, observer));

                if (0 > index)
                {
                    continue;
                }

                observers.RemoveAt(index);

                break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private sealed class ModalSubscription : IDisposable
        {
            private BootstrapModalService service;
            private IModalContentObserver observer;
            private bool disposed;

            public IModalContentObserver Observer => observer;

            public ModalSubscription(BootstrapModalService service, IModalContentObserver observer)
            {
                this.service = service;
                this.observer = observer;
            }

            public void Dispose()
            {
                Dispose(true);
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
                        service.ReleaseObserver(observer);

                        service = null;
                        observer = null;
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