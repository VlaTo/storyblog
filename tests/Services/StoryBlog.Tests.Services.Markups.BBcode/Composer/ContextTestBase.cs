using System.IO;
using System.Text;
using StoryBlog.Web.Services.Shared.BBCode.Composing;
using StoryBlog.Web.Services.Shared.BBCode.Parsing;

namespace StoryBlog.Tests.Services.Markups.BBcode.Composer
{
    public abstract class ContextTestBase : TestBase
    {
        protected abstract string Content { get; }

        protected BulletingBoardDocument Document { get; private set; }

        protected StringBuilder StringBuilder { get; private set; }

        protected override void Arrange()
        {
            Document = new BulletingBoardDocument();
            StringBuilder = new StringBuilder();
        }

        protected override void Act()
        {
            Document.Parse(Content);

            //var composer = new BulletingBoardMarkupComposer(StringBuilder);
            var writer = new StringWriter(StringBuilder);
            using var composer = new HtmlMarkupComposer(writer);
            composer.Visit(Document);
        }
    }
}
