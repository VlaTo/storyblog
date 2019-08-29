using StoryBlog.Web.Services.Blog.Interop.Markups.Parsing;

namespace StoryBlog.Tests.Services.Markups.BBcode.Parser
{
    public abstract class ContextTestBase : TestBase
    {
        protected abstract string Content { get; }

        protected BulletingBoardDocument Document { get; private set; }

        protected override void Arrange()
        {
            Document = new BulletingBoardDocument();
        }

        protected override void Act()
        {
            Document.Parse(Content);
        }
    }
}
