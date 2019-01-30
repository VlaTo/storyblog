using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;
using StoryBlog.Web.Services.Blog.Infrastructure.Annotations;

namespace StoryBlog.Web.Services.Blog.Infrastructure
{
    public abstract class FlagParser
    {
        protected abstract char Separator
        {
            get;
        }

        protected virtual IEqualityComparer<string> Comparer { get; } = StringComparer.OrdinalIgnoreCase;

        public void Parse(string text)
        {
            if (null == text)
            {
                throw new ArgumentNullException(nameof(text));
            }

            var segments = text.Split(Separator, StringSplitOptions.RemoveEmptyEntries);
            var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<KeyAttribute>();

                if (null == attribute)
                {
                    continue;
                }

                if (property.CanWrite && property.CanRead)
                {
                    var key = attribute.Name ?? property.Name;
                    var contains = segments.Contains(key, Comparer);

                    property.SetValue(this, contains);
                }
            }
        }
    }
}