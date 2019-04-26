using Microsoft.Extensions.Logging;
using System;

namespace StoryBlog.Web.Services.Blog.API.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="id"></param>
        public static void StoryCreated(this ILogger logger, long id)
        {
            if (null == logger)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogDebug($"New story was created (id: \'{id}\')");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="id"></param>
        public static void StoryUpdated(this ILogger logger, long id)
        {
            if (null == logger)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogDebug($"Story was updated (id: \'{id}\')");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="id"></param>
        public static void StoryDeleted(this ILogger logger, long id)
        {
            if (null == logger)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogDebug($"Story was deleted (id: \'{id}\')");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="id"></param>
        public static void CommentCreated(this ILogger logger, string slug, long id)
        {
            if (null == logger)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogDebug($"Story was deleted (id: \'{id}\')");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="id"></param>
        public static void CommentUpdated(this ILogger logger, string slug, long id)
        {
            if (null == logger)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogDebug($"Story was deleted (id: \'{id}\')");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="id"></param>
        public static void CommentDeleted(this ILogger logger, long id)
        {
            if (null == logger)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            logger.LogDebug($"Story was deleted (id: \'{id}\')");
        }
    }
}
