using StoryBlog.Web.Services.Shared.BBCode.Composing;
using StoryBlog.Web.Services.Shared.BBCode.Parsing;
using System;
using System.IO;
using System.Text;

namespace StoryBlog.Web.Services.Shared.BBCode
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

            using (var composer = new HtmlMarkupComposer(new StringWriter(stringBuilder)))
            {
                composer.Visit(document);
            }

            return stringBuilder.ToString();
        }
    }
}