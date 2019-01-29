namespace StoryBlog.Web.Services.Shared.Data.Csv
{
    public enum CsvNodeType
    {
        Field,
        Row,
        Document
    }

    public class CsvNode
    {
        public virtual CsvNodeType NodeType
        {
            get;
        }

        protected CsvNode(CsvNodeType nodeType)
        {
            NodeType = nodeType;
        }
    }
}