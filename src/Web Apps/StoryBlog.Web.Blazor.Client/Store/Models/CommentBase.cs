namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CommentBase
    {
        /// <summary>
        /// 
        /// </summary>
        public Comment Parent
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        protected CommentBase(Comment parent)
        {
            Parent = parent;
        }
    }
}