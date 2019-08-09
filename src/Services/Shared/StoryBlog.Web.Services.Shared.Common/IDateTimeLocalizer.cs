using System;

namespace StoryBlog.Web.Services.Shared.Common
{
    public interface IDateTimeLocalizer
    {
        string this[DateTime dateTime]
        {
            get;
        }
    }
}