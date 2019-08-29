using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StoryBlog.Tests.Services.Markups.BBcode
{
    [TestClass]
    public sealed class ParserTest : ContextTestBase
    {
        protected override string Content => "lorem [b]ipsum[\b] dolor [b][i]sit[\\i][\\b]";

        [TestMethod]
        public void HasNode()
        {
            Assert.IsNotNull(Node);
        }
    }
}