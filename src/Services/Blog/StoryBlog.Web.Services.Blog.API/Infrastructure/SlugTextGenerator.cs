using System;
using System.Text;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SlugTextGenerator : ISlugGenerator
    {
        /// <inheritdoc cref="ISlugGenerator.CreateFrom" />
        public string CreateFrom(string title)
        {
            const char hyphen = '-';
            var slug = new StringBuilder();

            foreach (var current in title)
            {
                var ch = Char.IsUpper(current) ? Char.ToLower(current) : current;

                if (false == Char.IsLetterOrDigit(ch))
                {
                    ch = hyphen;
                }

                if (hyphen == ch && slug[slug.Length - 1] == hyphen)
                {
                    continue;
                }

                slug.Append(ch);
            }

            return slug.ToString();
        }
    }
}