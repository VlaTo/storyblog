namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public partial class CsvRow : CsvNode
    {
        public CsvFieldCollection Fields
        {
            get;
        }

        public CsvRow()
            : base(CsvNodeType.Row)
        {
            Fields = new FieldCollection(this);
        }
    }
}