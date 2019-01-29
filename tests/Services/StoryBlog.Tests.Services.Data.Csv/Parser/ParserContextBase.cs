using System;
using System.IO;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Shared.Data.Csv;

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

        protected override Task ArrangeAsync()
        {
            var tokenizer = new CsvTokenizer(TextReader);

            Parser = new CsvParser(tokenizer);

            return Task.CompletedTask;
        }

        public override void Cleanup()
        {
            TextReader.Dispose();
        }
    }
}