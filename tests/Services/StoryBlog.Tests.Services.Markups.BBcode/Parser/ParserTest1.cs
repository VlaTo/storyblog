using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Services.Shared.BBCode.Nodes;

namespace StoryBlog.Tests.Services.Markups.BBcode.Parser
{
    [TestClass]
    public sealed class ParserTest1 : ContextTestBase
    {
        protected override string Content => "lorem [strong]ipsum[/strong] dolor [strong][underline]sit[/underline][/strong] amet!";

        [TestMethod]
        public void HasDocument()
        {
            Assert.IsNotNull(Document);
            Assert.AreEqual(BulletingBoardBlockType.Document, Document.BlockType);
        }

        [TestMethod]
        public void HasCorrectElementsCount()
        {
            Assert.AreEqual(5, Document.Elements.Count);
        }

        [TestMethod]
        public void HasBulletingBoardTextAtIndex0()
        {
            var element = Document.Elements.ElementAt(0);
            Assert.IsInstanceOfType(element, typeof(BulletingBoardText));
        }

        [TestMethod]
        public void HasBulletingBoardBlockAtIndex1()
        {
            var element = Document.Elements.ElementAt(1);
            Assert.IsInstanceOfType(element, typeof(BulletingBoardBlock));

            var block = (BulletingBoardBlock) element;
            Assert.IsNull(block.Argument);
            Assert.AreEqual(1, block.Elements.Count);
        }
    }
}