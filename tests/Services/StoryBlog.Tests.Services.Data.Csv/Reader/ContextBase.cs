using System.IO;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Shared.Data.Csv;

namespace StoryBlog.Tests.Services.Data.Csv.Reader
{
    public abstract class ContextBase : TestBase
    {
        protected abstract string Text
        {
            get;
        }

        protected CsvDocument Document;
        
        protected override Task ArrangeAsync()
        {
            return Task.CompletedTask;
        }

        protected override async Task ActAsync()
        {
            using (var reader = new StringReader(Text))
            {
                Document = await CsvDocument.CreateFromAsync(reader);
            }
        }
    }
}