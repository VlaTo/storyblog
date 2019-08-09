using System;

namespace StoryBlog.Web.Services.Shared.Common
{
    public interface ITimeSpanLocalizer
    {
        string this[TimeSpan timeSpan]
        {
            get;
        }
    }
}