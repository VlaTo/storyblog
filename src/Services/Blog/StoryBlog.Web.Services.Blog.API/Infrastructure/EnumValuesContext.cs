using System;
using System.Reflection;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    internal sealed class EnumValuesContext
    {
        public Type EnumType { get; }

        public string Separator { get; }

        public EnumValuesContext(Type enumType, string separator)
        {
            EnumType = enumType;
            Separator = separator;
        }
    }
}