using System;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    public abstract class StoryBase
    {
        public long Id
        {
            get;
        }

        public string Slug
        {
            get;
            set;
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

        public DateTime Created
        {
            get;
            set;
        }

        public DateTime? Modified
        {
            get;
            set;
        }

        protected StoryBase(long id)
        {
            Id = id;
        }
    }
}