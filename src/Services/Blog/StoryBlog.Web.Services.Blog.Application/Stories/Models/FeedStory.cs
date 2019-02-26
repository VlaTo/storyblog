namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
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

        public FeedStory(long id) 
            : base(id)
        {
        }
    }
}