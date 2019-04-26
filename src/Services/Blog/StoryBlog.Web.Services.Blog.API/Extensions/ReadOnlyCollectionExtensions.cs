using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.API.Extensions
{
    internal static class ReadOnlyCollectionExtensions
    {
        public static int FindIndex<T>(this IReadOnlyCollection<T> collection, Predicate<T> predicate)
        {
            if (null == collection)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (null == predicate)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (0 < collection.Count)
            {
                var index = 0;

                foreach (var item in collection)
                {
                    if (predicate.Invoke(item))
                    {
                        return index;
                    }

                    index++;
                }
            }

            return -1;
        }

        public static int FindIndex<T>(this IReadOnlyCollection<T> collection, T item)
        {
            var comparer = EqualityComparer<T>.Default;
            return FindIndex(collection, source => comparer.Equals(source, item));
        }
    }
}