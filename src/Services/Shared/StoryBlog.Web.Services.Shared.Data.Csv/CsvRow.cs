namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public partial class CsvRow : CsvNode
    {
        internal ICsvDocument Document
        {
            get;
        }

        public CsvFieldCollection Fields
        {
            get;
        }

        public CsvRow(CsvDocument document)
            : base(CsvNodeType.Row)
        {
            Document = document;
            Fields = new FieldCollection(this);
        }
    }
}