namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentCreated : CommentBase
    {
        /// <summary>
        /// 
        /// </summary>
        public long? Parent
        {
            get;
            set;
        }
    }
}