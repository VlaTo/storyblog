using System;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Comment
    {
        public long Id
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public int AuthorIndex
        {
            get;
            set;
        }

        public long? ParentId
        {
            get; set;
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
    }
}