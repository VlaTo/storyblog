using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Services.Shared.Data.Csv;

namespace StoryBlog.Tests.Services.Data.Csv.Parser.SingleLine
{
    [TestClass]
    public sealed class SimpleOneLine : ParserContextBase
    {
        protected override TextReader TextReader => new StringReader("test1");

        internal CsvRow Row
        {
            get;
            private set;
        }

        [TestMethod]
        public void Test1()
        {
            Assert.IsNotNull(Row);
        }

        [TestMethod]
        public void Test2()
        {
            Assert.AreEqual(1, Row.Fields.Count());
        }

        [TestMethod]
        public void Test3()
        {
            var field = Row.Fields.First();
            Assert.AreEqual("test1", field.Text);
        }

        protected override async Task ActAsync()
        {
            var document = new CsvDocument();

            await Parser.ParseInternalAsync(document, new CsvParsingOptions());

            if (1 == document.Rows.Count)
            {
                Row = document.Rows[0];
            }
        }
    }
}