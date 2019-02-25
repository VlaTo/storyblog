using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Primitives;
using StoryBlog.Web.Services.Shared.Common.Annotations;

namespace StoryBlog.Web.Services.Shared.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumFlags
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFlags"></typeparam>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static TFlags Parse<TFlags>(IEnumerable<string> strings)
            where TFlags : struct
        {
            return Parse<TFlags>(strings, StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFlags"></typeparam>
        /// <param name="strings"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static TFlags Parse<TFlags>(IEnumerable<string> strings, IEqualityComparer<string> comparer)
            where TFlags : struct
        {
            if (null == strings)
            {
                throw new ArgumentNullException(nameof(strings));
            }

            if (null == comparer)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            var enumType = typeof(TFlags);
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var values = Enum.GetValues(enumType);
            var hash = new Dictionary<string, TFlags>(comparer);

            foreach (var value in values)
            {
                if (false == Enum.IsDefined(enumType, value))
                {
                    throw new Exception();
                }

                var name = Enum.GetName(enumType, value);
                var property = fields.FirstOrDefault(field => comparer.Equals(field.Name, name));
                var attribute = property?.GetCustomAttribute<FlagAttribute>();

                hash[attribute?.Key ?? name] = (TFlags) value;
            }

            var keys = StringValues.Empty;

            foreach (var key in strings)
            {
                if (false == hash.TryGetValue(key, out var value))
                {
                    throw new Exception();
                }

                keys = StringValues.Concat(keys, value.ToString());
            }

            return (TFlags) Enum.Parse(enumType, keys.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFlags"></typeparam>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static StringValues ToQueryString<TFlags>(TFlags flags)
            where TFlags : struct
            => ToQueryString(flags, StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFlags"></typeparam>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static StringValues ToQueryString<TFlags>(TFlags flags, IEqualityComparer<string> comparer)
            where TFlags : struct
        {
            var enumType = typeof(TFlags);
            //var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            var separator = CultureInfo.InvariantCulture.TextInfo.ListSeparator;
            var fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var names = flags.ToString().Split(new[] {separator}, StringSplitOptions.RemoveEmptyEntries);
            var values = StringValues.Empty;

            foreach (var name in names)
            {
                var key = name.Trim();
                var property = fields.FirstOrDefault(field => comparer.Equals(field.Name, key));
                var attribute = property?.GetCustomAttribute<FlagAttribute>();
                values = StringValues.Concat(values, attribute?.Key ?? key);
            }

            return values;
        }
    }
}