namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Comment : CommentBase<Comment>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public Comment(Comment parent)
            : base(parent)
        {
        }
    }
}