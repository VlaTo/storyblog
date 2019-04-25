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
        public static bool IsEmpty<TData, TResources>(this IQueryResult<TData, TResources> result)
            where TResources : IQueryResultResources
        {
            if (null == result)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return false == (result.Exceptions.Any() || result.Data.Any());
        }
    }
}