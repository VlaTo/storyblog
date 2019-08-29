using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Services.Blog.Interop.Markups;

namespace StoryBlog.Tests.Services.Markups.BBcode
{
    public abstract class ContextTestBase : TestBase
    {
        protected abstract string Content { get; }

        protected MarkupNode Node { get; private set; }

        protected override void Arrange()
        {

        }

        protected override void Act()
        {
            Node = BBCodeMarkup.Parse(Content);
        }
    }
}
