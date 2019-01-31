namespace StoryBlog.Web.Services.Blog.Application.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NavigationCursor
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        public NavigationCursor(long id, int count)
        {
            Id = id;
            Count = count;
        }
    }
}