using System;
using System.Collections.Generic;
using System.Linq;

namespace StoryBlog.Web.Services.Blog.Application.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var item in enumerable)
            {
                action.Invoke(item);
                yield return item;
            }
        }

        public static IList<T> AddRange<T>(this IList<T> list, IEnumerable<T> enumerable)
        {
            if (null == list)
            {
                throw new ArgumentNullException(nameof(list));
            }

            foreach (var item in enumerable ?? Enumerable.Empty<T>())
            {
                list.Add(item);
            }

            return list;
        }
    }
}