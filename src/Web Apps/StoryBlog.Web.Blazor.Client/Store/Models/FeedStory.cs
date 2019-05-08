namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FeedStory : StoryBase
    {
        /// <summary>
        /// 
        /// </summary>
        public int CommentsCount
        {
            get;
            set;
        }
    }
}