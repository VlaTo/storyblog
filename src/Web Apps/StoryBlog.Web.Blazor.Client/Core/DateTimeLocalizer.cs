using System;
using System.Globalization;
using StoryBlog.Web.Services.Shared.Common;

namespace StoryBlog.Web.Blazor.Client.Core
{
    internal sealed class DateTimeLocalizer : IDateTimeLocalizer
    {
        public string this[DateTime dateTime]
        {
            get
            {
                return dateTime.ToString("g", CultureInfo.CurrentUICulture);
            }
        }

        public DateTimeLocalizer()
        {
        }
    }
}