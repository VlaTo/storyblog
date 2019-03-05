using System.IO;
using System.Reflection;

namespace StoryBlog.Tests.Services.Data.Csv.Reader.Create
{
    public abstract class MethodBase : ContextBase
    {
        protected override TextReader Reader
        {
            get
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StoryBlog.Tests.Services.Data.Csv.Reader.Create.Authors.csv");
                return new StreamReader(stream);
            }
        }
    }
}