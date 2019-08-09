using System;
using System.Globalization;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Blazor.Client.Core
{
    internal sealed class TimeSpanLocalizer : ITimeSpanLocalizer
    {
        public string this[TimeSpan timeSpan]
        {
            get
            {
                return timeSpan.ToString("g", CultureInfo.CurrentUICulture);
            }
        }

        public TimeSpanLocalizer()
        {
        }
    }
}