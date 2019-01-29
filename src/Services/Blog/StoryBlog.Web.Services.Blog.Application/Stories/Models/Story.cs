using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    public sealed class Story
    {
        public long Id
        {
            get;
        }

        public string Title
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public Author Author
        {
            get;
            set;
        }

        public IList<Comment> Comments
        {
            get;
        }

        public Story(long id)
        {
            Id = id;
            Comments = new List<Comment>();
        }
    }
}