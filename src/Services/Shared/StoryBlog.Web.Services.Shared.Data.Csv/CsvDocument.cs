using System;
using System.Collections.Generic;
using System.IO;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public partial class CsvDocument : CsvNode, ICsvDocument
    {
        private readonly RowCollection rows;
        private readonly NameCollection names;

        public ICollection<string> Names => names;

        public IList<CsvRow> Rows => rows;

        internal static Lazy<CsvParsingOptions> DefaultOptions;

        RowCollection ICsvDocument.RowsCollection => rows;

        NameCollection ICsvDocument.NamesCollection => names;

        internal CsvDocument()
            : base(CsvNodeType.Document)
        {
            names = new NameCollection(this);
            rows = new RowCollection(this);
        }

        static CsvDocument()
        {
            DefaultOptions = new Lazy<CsvParsingOptions>(() => new CsvParsingOptions
            {
                HasHeader = true
            });
        }

        public static CsvDocument CreateFrom(TextReader reader, CsvParsingOptions options = null)
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
                    parser.Parse(document, options ?? DefaultOptions.Value);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return document;
        }

        internal CsvRow CreateRow()
        {
            return new CsvRow(this);
        }

        internal CsvField CreateField(CsvRow row, string text)
        {
            return new CsvField(row, text);
        }
    }
}
