using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.RenderTree;

namespace StoryBlog.Web.Blazor.Components
{
    /// <summary>
    /// Time label class.
    /// </summary>
    public class TimeLabel : BlazorComponent, IDisposable
    {
        internal delegate void ContentUpdaterCallback(DateTime dateTime);

        private static readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(20.0d);
        private static readonly ContentUpdater updater;

        private DateTime dateTime;
        private IDisposable subscription;
        private string content;

        /// <summary>
        /// 
        /// </summary>
        [Parameter]
        protected DateTime DateTime
        {
            get => dateTime;
            set
            {
                if (dateTime == value)
                {
                    return;
                }

                dateTime = value;

                StateHasChanged();
            }
        }

        static TimeLabel()
        {
            updater = new ContentUpdater(defaultTimeout);
        }

        void IDisposable.Dispose()
        {
            subscription.Dispose();
        }

        protected override void OnInit()
        {
            base.OnInit();

            subscription = updater.Register(OnTimerCallback);
            UpdateContent(DateTime.Now, false);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            builder.OpenElement(0, "time");
            builder.AddAttribute(1, "datetime", DateTime.Now.ToString("u"));
            builder.AddContent(2, content);
            builder.CloseElement();
        }

        private void OnTimerCallback(DateTime now)
        {
            UpdateContent(now, true);
        }

        private void UpdateContent(DateTime now, bool invalidateContent)
        {
            content = GetTimeContent(now);

            if (invalidateContent)
            {
                StateHasChanged();
            }
        }

        private string GetTimeContent(DateTime dateTime)
        {
            var interval = dateTime - DateTime;

            if (interval < TimeSpan.FromSeconds(3.0d))
            {
                return "just second ago";
            }

            if (interval < TimeSpan.FromMinutes(1.0d))
            {
                var seconds = (int)interval.TotalSeconds;
                return String.Format($"a {seconds} seconds ago");
            }

            if (interval < TimeSpan.FromHours(1.0d))
            {
                var minutes = interval.TotalMinutes;
                return String.Format($"a {minutes:F1} minutes ago");
            }

            if (interval < TimeSpan.FromDays(1.0d))
            {
                var hours = interval.TotalHours;
                return String.Format($"a {hours:F1} hours ago");
            }

            return String.Format($"at {DateTime.Date:d}");
        }

        /// <summary>
        /// 
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

            public IDisposable Register(ContentUpdaterCallback callback)
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