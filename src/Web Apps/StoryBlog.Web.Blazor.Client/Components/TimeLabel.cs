using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace StoryBlog.Web.Client.Components
{
    /// <summary>
    /// Time label class.
    /// </summary>
    public class TimeLabel : BootstrapComponentBase, IDisposable
    {
        internal delegate void ContentUpdaterCallback(DateTime dateTime);

        private static readonly TimeSpan defaultTimeout;
        private static readonly TimeSpan spanUpdateDuration;
        private static readonly ContentUpdater updater;

        private IDisposable subscription;
        private string content;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public DateTime DateTime
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        public Func<TimeLabel, TimeSpan, string> ConvertFallback
        {
            get;
            set;
        }

        static TimeLabel()
        {
            defaultTimeout = TimeSpan.FromSeconds(20.0d);
            spanUpdateDuration = TimeSpan.FromDays(1.0d);
            updater = new ContentUpdater(defaultTimeout);
        }

        void IDisposable.Dispose()
        {
            subscription?.Dispose();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            subscription = updater.Subscribe(OnTimerCallback);

            UpdateContent(DateTime.Now, false);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, "time");

            builder.AddAttribute(1, "datetime", DateTime.ToString("u"));
            builder.AddContent(2, content);

            builder.CloseElement();
        }

        private void OnTimerCallback(DateTime now)
        {
            UpdateContent(now, true);
        }

        private void UpdateContent(DateTime now, bool invalidateContent)
        {
            var span = now - DateTime;

            if (span < TimeSpan.Zero)
            {
                return;
            }

            if (null == subscription)
            {
                return;
            }

            content = null == ConvertFallback
                ? DateTime.ToString("f", CultureInfo.CurrentUICulture)
                : ConvertFallback.Invoke(this, span);

            if (span > spanUpdateDuration)
            {
                subscription.Dispose();
                subscription = null;
            }

            if (invalidateContent)
            {
                StateHasChanged();
            }
        }

        /// <summary>
        /// Content updater by timer.
        /// </summary>
        private class ContentUpdater : IDisposable
        {
            private readonly TimeSpan timeout;
            private readonly IList<Subscription> subscriptions;
            private readonly object gate;
            private Timer timer;

            public ContentUpdater(TimeSpan timeout)
            {
                this.timeout = timeout;
                gate = new object();
                subscriptions = new List<Subscription>();
            }

            public IDisposable Subscribe(ContentUpdaterCallback callback)
            {
                var subscription = new Subscription(this, callback);

                lock (gate)
                {
                    if (null == timer)
                    {
                        timer = new Timer(OnUpdateCallback);
                        timer.Change(timeout, timeout);
                    }

                    subscriptions.Add(subscription);
                }

                return subscription;
            }

            public void Dispose()
            {
                lock (gate)
                {
                    while (0 < subscriptions.Count)
                    {
                        var subscription = subscriptions[subscriptions.Count - 1];
                        RemoveSubscription(subscription);
                    }
                }
            }

            private void OnUpdateCallback(object state)
            {
                var now = DateTime.Now;
                var subscribers = subscriptions.ToArray();

                foreach (var subscription in subscribers)
                {
                    subscription.InvokeCallback(now);
                }
            }

            private void Unregister(Subscription subscription)
            {
                lock (gate)
                {
                    RemoveSubscription(subscription);
                }
            }

            private void RemoveSubscription(Subscription subscription)
            {
                subscriptions.Remove(subscription);

                if (0 == subscriptions.Count)
                {
                    timer.Dispose();
                    timer = null;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            private class Subscription : IDisposable
            {
                private readonly ContentUpdater updater;
                private readonly ContentUpdaterCallback callback;

                public Subscription(ContentUpdater updater, ContentUpdaterCallback callback)
                {
                    this.updater = updater;
                    this.callback = callback;
                }

                public void InvokeCallback(DateTime dateTime)
                {
                    callback.Invoke(dateTime);
                }

                public void Dispose()
                {
                    updater.Unregister(this);
                }
            }
        }
    }
}