using System;

namespace StoryBlog.Web.Client.Extensions
{
    internal static class StringExtensions
    {
        public static bool EndsWith(this string str, char ch)
        {
            if (null == str)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (0 == str.Length)
            {
                return false;
            }

            var position = str.Length - 1;

            return ch == str[position];
        }
    }
}