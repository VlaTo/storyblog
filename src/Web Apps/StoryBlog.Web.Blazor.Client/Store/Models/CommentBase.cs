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

        public string StorySlug
        {
            get; 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        protected CommentBase(string storySlug, Comment parent)
        {
            StorySlug = storySlug;
            Parent = parent;
        }
    }
}