using StoryBlog.Web.Services.Shared.Data.Csv;
using System.IO;

namespace StoryBlog.Tests.Services.Data.Csv.Reader
{
    public abstract class ContextBase : TestBase
    {
        protected abstract TextReader Reader
        {
            get;
        }

        protected abstract CsvParsingOptions Options
        {
            get;
        }

        protected CsvDocument Document;
        
        protected override void Act()
        {
            using (Reader)
            {
                Document = CsvDocument.CreateFrom(Reader, Options);
            }
        }
    }
}