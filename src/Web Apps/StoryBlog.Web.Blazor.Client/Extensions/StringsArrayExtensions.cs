using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Extensions
{
    internal static class StringsArrayExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strings"></param>
        /// <returns></returns>
        public static string ToScopes(this IEnumerable<string> strings)
        {
            if (null == strings)
            {
                throw new ArgumentNullException(nameof(strings));
            }

            return String.Join(" ", strings);
        }
    }
}
