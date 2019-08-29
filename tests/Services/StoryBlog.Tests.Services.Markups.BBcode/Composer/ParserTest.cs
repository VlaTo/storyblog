using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StoryBlog.Tests.Services.Markups.BBcode.Composer
{
    [TestClass]
    public sealed class ParserTest : ContextTestBase
    {
        protected override string Content => "lorem [strong]ipsum[/strong] dolor [strong][underline]sit[/underline][/strong] amet!";

        [TestMethod]
        public void HasStringBuilder()
        {
            Assert.IsNotNull(StringBuilder);
        }

        [TestMethod]
        public void GeneratedEqualsToSource()
        {
            const string expected = "lorem <b>ipsum</b> dolor <b><i>sit</i></b> amet!";
            Assert.AreEqual(expected, StringBuilder.ToString());
        }
    }
}