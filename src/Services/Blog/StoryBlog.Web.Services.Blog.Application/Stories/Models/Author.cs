namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    public sealed class Author
    {
        public long Id
        {
            get;
        }

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