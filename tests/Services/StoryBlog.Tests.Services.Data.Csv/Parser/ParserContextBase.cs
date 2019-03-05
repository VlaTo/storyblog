using StoryBlog.Web.Services.Shared.Data.Csv;
using System.IO;

namespace StoryBlog.Tests.Services.Data.Csv.Parser
{
    public abstract class ParserContextBase : TestBase
    {
        protected abstract TextReader TextReader
        {
            get;
        }

        internal CsvParser Parser
        {
            get;
            set;
        }

        protected override void Arrange()
        {
            var tokenizer = new CsvTokenizer(TextReader);
            Parser = new CsvParser(tokenizer);
        }

        public override void Cleanup()
        {
            TextReader.Dispose();
        }
    }
}