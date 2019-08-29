namespace StoryBlog.Web.Services.Blog.Interop.Markups
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MarkupVisitor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public abstract void Visit(MarkupNode node);
    }
}