using System;
using System.Collections.Generic;

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
    }
}