using System;
using System.Text;
using StoryBlog.Web.Services.Blog.Interop.Markups.Composing;
using StoryBlog.Web.Services.Blog.Interop.Markups.Parsing;

namespace StoryBlog.Web.Services.Blog.Interop.Markups
{
    public static class BulletingBoardDocumentExtensions
    {
        public static string ToHtml(this BulletingBoardDocument document)
        {
            if (null == document)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var stringBuilder = new StringBuilder();

            using (var composer = new HtmlMarkupComposer(stringBuilder))
            {
                composer.Visit(document);
            }

            return stringBuilder.ToString();
        }
    }
}