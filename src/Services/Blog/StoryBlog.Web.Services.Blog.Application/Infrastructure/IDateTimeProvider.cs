using System;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// 
        /// </summary>
        DateTime UtcNow
        {
            get;
        }
    }
}
