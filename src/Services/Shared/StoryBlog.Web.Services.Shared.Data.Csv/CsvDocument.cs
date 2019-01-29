using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public partial class CsvDocument : CsvNode
    {
        private readonly RowCollection rows;
        private readonly NameCollection names;

        public ICollection<string> Names
        {
            get;
            internal set;
        }

        public IList<CsvRow> Rows => rows;

        internal CsvDocument()
            : base(CsvNodeType.Document)
        {
            names = new NameCollection(this);
            rows = new RowCollection(this);
        }

        public static async Task<CsvDocument> CreateFromAsync(TextReader reader, CsvParsingOptions options = null)
        {
            if (null == reader)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var document = new CsvDocument();

            using (var tokenizer = new CsvTokenizer(reader))
            {
                try
                {
                    var parser = new CsvParser(tokenizer);
                    await parser.ParserAsync(document, options);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return document;
        }

        internal CsvField CreateField(string text)
        {
            return new CsvField(this, text);
        }
    }
}
