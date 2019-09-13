using StoryBlog.Web.Services.Shared.Common;
using System;
using System.Globalization;

namespace StoryBlog.Web.Client.Core
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