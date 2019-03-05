using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Services.Shared.Data.Csv;

namespace StoryBlog.Tests.Services.Data.Csv.Reader.Create
{
    [TestClass]
    public sealed class Rows1 : MethodBase
    {
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
            Assert.AreEqual(2, Document.Names.Count);
        }

        [TestMethod]
        public void Test4()
        {
            var names = Document.Names.ToArray();
            Assert.AreEqual("Id", names[0]);
        }

        [TestMethod]
        public void Test5()
        {
            var names = Document.Names.ToArray();
            Assert.AreEqual("UserName", names[1]);
        }

        [TestMethod]
        public void Test6()
        {
            Assert.IsNotNull(Document.Rows);
        }

        [TestMethod]
        public void Test7()
        {
            Assert.AreEqual(4, Document.Rows.Count);
        }

        [TestMethod]
        public void Test8()
        {
            var names = Document.Names.ToArray();
            var row = Document.Rows[0];
            var field = row.Fields[0];
            Assert.AreEqual(names[0], field.Name);
        }
    }
}