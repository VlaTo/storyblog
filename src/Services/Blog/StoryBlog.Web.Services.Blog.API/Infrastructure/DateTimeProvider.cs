using System;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc cref="IDateTimeProvider.Now" />
        public DateTime Now => DateTime.Now;
    }
}
