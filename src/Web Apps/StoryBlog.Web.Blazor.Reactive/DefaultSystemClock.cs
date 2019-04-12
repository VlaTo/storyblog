using System;
using System.ComponentModel;

namespace StoryBlog.Web.Blazor.Reactive
{
    /// <summary>
    /// (Infrastructure) Provides access to the local system clock.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class DefaultSystemClock : ISystemClock
    {
        /// <summary>
        /// Gets the current time.
        /// </summary>
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
