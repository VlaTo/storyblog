using System;
using System.Text;

namespace StoryBlog.Web.Services.Shared.BBCode
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendIf(this StringBuilder stringBuilder, char ch, bool flag)
        {
            if (null == stringBuilder)
            {
                throw new ArgumentNullException(nameof(stringBuilder));
            }

            if (flag)
            {
                stringBuilder.Append(ch);
            }

            return stringBuilder;
        }
    }
}