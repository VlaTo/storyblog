using System;
using System.Text;

namespace StoryBlog.Web.Client.Extensions
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendIf(this StringBuilder builder, string text, bool condition)
        {
            if (null == builder)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (condition)
            {
                builder.Append(text);
            }

            return builder;
        }
    }
}