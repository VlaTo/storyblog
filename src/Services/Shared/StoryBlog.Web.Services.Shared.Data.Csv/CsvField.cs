using System;

namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public class CsvField : CsvNode
    {
        private readonly CsvDocument document;

        public string Text
        {
            get;
        }

        public string Name
        {
            get
            {
                /*if (null == document.Header)
                {
                    return null;
                }

                document.Header[]*/

                throw new NotImplementedException();
            }
        }

        public CsvField(CsvDocument document, string text)
            : base(CsvNodeType.Field)
        {
            this.document = document;
            Text = text;
        }
    }
}