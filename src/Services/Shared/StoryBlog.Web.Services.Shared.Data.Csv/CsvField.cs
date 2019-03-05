namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public class CsvField : CsvNode
    {
        public string Text
        {
            get;
        }

        public string Name
        {
            get
            {
                var document = Row.Document;

                if (0 == document.NamesCollection.Count)
                {
                    return null;
                }

                return document.NamesCollection[Index];
            }
        }

        public int Index => Row.Fields.FindIndex(this);

        internal CsvRow Row
        {
            get;
        }

        public CsvField(CsvRow row, string text)
            : base(CsvNodeType.Field)
        {
            Row = row;
            Text = text;
        }
    }
}