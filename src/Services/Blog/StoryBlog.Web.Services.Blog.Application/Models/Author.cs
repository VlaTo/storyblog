namespace StoryBlog.Web.Services.Blog.Application.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Author
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
        public string UserName
        {
            get;
        }

        public Author(long id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}