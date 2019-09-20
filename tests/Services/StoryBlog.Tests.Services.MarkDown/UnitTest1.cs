using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoryBlog.Web.Services.Shared.MarkDown;

namespace StoryBlog.Tests.Services.MarkDown
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var temp = await MarkDownDocument.ParseAsync("sample data\r\nanother sample data line");
        }

        [TestMethod]
        public async Task TestMethod3()
        {
            var temp = await MarkDownDocument.ParseAsync("_sample data_\n");
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            var document = await MarkDownDocument.ParseAsync("### sample _data_ line\r\nanother sample data line");

            var stream = new MemoryStream();
            var stringWriter = new StreamWriter(stream, Encoding.UTF8);
            using var writer = new HtmlWriter(stringWriter);
            var composer = new HtmlContentComposer(HtmlTagFactory.Instance, writer);

            composer.AddDecorator<MarkDownDocument>(new HtmlMarkDownDocumentTagDecorator());

            composer.Visit(document);
            stringWriter.Flush();

            var text = Encoding.UTF8.GetString(stream.ToArray());

            Debug.WriteLine(text);
        }
    }
}
