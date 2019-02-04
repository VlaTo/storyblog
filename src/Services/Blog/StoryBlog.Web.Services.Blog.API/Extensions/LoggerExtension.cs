using Microsoft.Extensions.Logging;
using System;

namespace StoryBlog.Web.Services.Blog.API.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoggerExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="id"></param>
        public static void NewStoryCreated(this ILogger logger, long id)
        {
            if (null == logger)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogDebug($"New story was created (id: \'{id}\')");
        }
    }
}
