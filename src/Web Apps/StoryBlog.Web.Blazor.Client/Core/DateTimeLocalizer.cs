using StoryBlog.Web.Services.Shared.Common;
using System;
using System.Globalization;

namespace StoryBlog.Web.Client.Core
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