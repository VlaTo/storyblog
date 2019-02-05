using Microsoft.EntityFrameworkCore.Internal;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using System;

namespace StoryBlog.Web.Services.Blog.Application.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class RequestResultExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsSuccess(this IRequestResult result)
        {
            if (null == result)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return false == result.Exceptions.Any();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsEmpty<TData>(this IQueryResult<TData> result)
        {
            if (null == result)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return false == (result.Exceptions.Any() || result.Entities.Any());
        }
    }
}