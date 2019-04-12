using StoryBlog.Web.Blazor.Reactive.Concurrency;
using System;

namespace StoryBlog.Web.Blazor.Reactive
{
    internal class DefaultSystemClockMonitor : PeriodicTimerSystemClockMonitor
    {
        private static readonly TimeSpan DefaultPeriod = TimeSpan.FromSeconds(1);

        public DefaultSystemClockMonitor()
            : base(DefaultPeriod)
        {
        }
    }
}
