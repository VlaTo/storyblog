namespace StoryBlog.Web.Services.Shared.MarkDown.Elements
{
    public sealed class MarkDownHeadingElement : MarkDownNode
    {
        public int Level
        {
            get;
        }

        public MarkDownHeadingElement(int level)
        {
            Level = level;
        }
    }
}