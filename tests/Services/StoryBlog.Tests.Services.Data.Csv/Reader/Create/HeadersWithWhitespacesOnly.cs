﻿using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Services.Shared.Data.Csv;

namespace StoryBlog.Tests.Services.Data.Csv.Reader.Create
{
    [TestClass]
    public sealed class HeadersWithWhitespacesOnly : ContextBase
    {
        protected override TextReader Reader => new StringReader("test");

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
            Assert.AreEqual(1, Document.Names.Count);
        }

        [TestMethod]
        public void Test4()
        {
            Assert.AreEqual("test", Document.Names.First());
        }

        [TestMethod]
        public void Test5()
        {
            Assert.IsNotNull(Document.Rows);
        }
    }
}