using System.Text;
using StoryBlog.Web.Services.Blog.Interop.Markups.Composing;
using StoryBlog.Web.Services.Blog.Interop.Markups.Parsing;

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
            using var composer = new HtmlMarkupComposer(StringBuilder);
            composer.Visit(Document);
        }
    }
}
