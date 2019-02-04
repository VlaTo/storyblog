using System;
using System.Collections.Generic;
using System.Reflection;
using StoryBlog.Web.Services.Blog.Infrastructure.Annotations;

namespace StoryBlog.Web.Services.Blog.Infrastructure
{
    public static class FlagParser
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFlags"></typeparam>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static TFlags Parse<TFlags>(IEnumerable<string> strings)
            where TFlags : class, new()
            => Parse<TFlags>(strings, StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFlags"></typeparam>
        /// <param name="strings"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static TFlags Parse<TFlags>(IEnumerable<string> strings, IEqualityComparer<string> comparer)
            where TFlags: class, new ()
        {
            if (null == strings)
            {
                throw new ArgumentNullException(nameof(strings));
            }

            if (null == comparer)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            var hash = new HashSet<string>(strings, comparer);
            var properties = typeof(TFlags).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var flags = new TFlags();

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<KeyAttribute>();

                if (null == attribute)
                {
                    continue;
                }

                if (property.CanWrite)
                {
                    var key = attribute.Name ?? property.Name;
                    var contains = hash.Contains(key);

                    property.SetValue(flags, contains);
                }
            }

            return flags;
        }

        public static string[] ToArray<TFlags>(TFlags flags) 
            where TFlags: class
        {
            if (null == flags)
            {
                throw new ArgumentNullException(nameof(flags));
            }

            var values = new List<string>();
            var properties = typeof(TFlags).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<KeyAttribute>();

                if (null == attribute)
                {
                    continue;
                }

                if (property.CanRead)
                {
                    var key = attribute.Name ?? property.Name;
                    var value = property.GetValue(flags);
                    var contains = Convert.ToBoolean(value);

                    if (false == contains)
                    {
                        continue;
                    }

                    values.Add(key);
                }
            }

            return values.ToArray();
        }

        public static string ToCommaSeparatedString<TFlags>(TFlags flags) 
            where TFlags: class
        {
            if (null == flags)
            {
                throw new ArgumentNullException(nameof(flags));
            }

            var values = ToArray(flags);

            return String.Join(',', values);
        }
    }
}