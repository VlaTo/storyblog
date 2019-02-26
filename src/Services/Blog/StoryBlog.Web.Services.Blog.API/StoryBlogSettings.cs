namespace StoryBlog.Web.Services.Blog.API
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryBlogSettings
    {
        /// <summary>
        /// Gets or sets page size amount.
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int FeaturedStoriesCount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int FeedStoriesCount
        {
            get;
            set;
        }
    }
}