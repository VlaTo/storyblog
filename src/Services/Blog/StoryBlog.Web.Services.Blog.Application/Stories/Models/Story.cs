using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    public sealed class Story
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

        public Author Author
        {
            get;
            set;
        }

        public IList<Comment> Comments
        {
            get;
        }

        public bool IsPublic
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

        public Story(long id)
        {
            Id = id;
            Comments = new List<Comment>();
        }
    }
}