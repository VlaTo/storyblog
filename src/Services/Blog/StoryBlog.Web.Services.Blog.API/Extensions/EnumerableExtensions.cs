using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryBlog.Web.Services.Blog.API.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            if (null == source)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var list = new List<T>(source);

            return new ReadOnlyCollection<T>(list);
        }
    }
}