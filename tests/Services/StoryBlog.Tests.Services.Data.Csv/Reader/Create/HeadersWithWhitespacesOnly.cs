using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StoryBlog.Tests.Services.Data.Csv.Reader.Create
{
    [TestClass]
    public sealed class HeadersWithWhitespacesOnly : ContextBase
    {
        protected override string Text => "test,test1, test2 , test3,=\"030810023\"";

        [TestMethod]
        public void Test1()
        {
            Assert.IsNotNull(Document);
        }
    }
}