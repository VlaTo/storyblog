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

        /// <summary>
        /// 
        /// </summary>
        public Author Author
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