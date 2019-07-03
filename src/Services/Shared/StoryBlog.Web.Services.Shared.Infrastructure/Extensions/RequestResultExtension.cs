using System;
using System.Linq;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Extensions
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
        public static bool IsEmpty<TData>(this IQueryResult<TData> result)
        {
            if (null == result)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return false == result.Entities.Any();
        }
    }
}