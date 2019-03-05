namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    internal interface ICsvDocument
    {
        CsvDocument.RowCollection RowsCollection
        {
            get;
        }

        CsvDocument.NameCollection NamesCollection
        {
            get;
        }
    }
}