using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StoryBlog.Tests.Services.Markups.BBcode
{
    [TestClass]
    public sealed class ParserTest1 : ContextTestBase
    {
        protected override string Content => "lorem ipsum dolor sit amet.";

        [TestMethod]
        public void HasNode()
        {
            Assert.IsNotNull(Node);
        }
    }
}