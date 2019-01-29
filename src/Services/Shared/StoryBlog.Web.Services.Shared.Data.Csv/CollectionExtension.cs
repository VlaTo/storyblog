using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public static class CollectionExtension
    {
        public static ICollection<T> ForEach<T>(this ICollection<T> collection, Action<T> action)
        {
            if (null == collection)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in collection)
            {
                action.Invoke(item);
            }

            return collection;
        }
    }
}