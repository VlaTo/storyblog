namespace StoryBlog.Web.Services.Shared.MarkDown.Elements
{
    /// <summary>
    /// 
    /// </summary>
    public enum EmphasisType
    {
        Italic,
        Bold
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class MarkDownEmphasisElement : MarkDownNode
    {
        public EmphasisType EmphasisType
        {
            get;
        }

        protected MarkDownEmphasisElement(EmphasisType emphasisType)
        {
            EmphasisType = emphasisType;
        }
    }
}