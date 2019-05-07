using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Services.Shared.Data.Csv;

namespace StoryBlog.Tests.Services.Data.Csv.Reader.Create
{
    [TestClass]
    public class Headers1 : ContextBase
    {
        protected override TextReader Reader => new StringReader("test,test1, test2 , test3,=\"030810023\"");

        protected override CsvParsingOptions Options => new CsvParsingOptions
        {
            HasHeader = true
        };

        protected override void Arrange()
        {
        }

        [TestMethod]
        public void Test1()
        {
            Assert.IsNotNull(Document);
        }

        [TestMethod]
        public void Test2()
        {
            Assert.IsNotNull(Document.Names);
        }

        [TestMethod]
        public void Test3()
        {
            Assert.AreEqual(5, Document.Names.Count);
        }

        [TestMethod]
        public void Test4()
        {
            var names = Document.Names.ToArray();
            Assert.AreEqual("test", names[0]);
        }

        [TestMethod]
        public void Test5()
        {
            var names = Document.Names.ToArray();
            Assert.AreEqual("test1", names[1]);
        }

        [TestMethod]
        public void Test6()
        {
            Assert.IsNotNull(Document.Rows);
        }
    }
}