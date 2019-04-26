using System;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class StoryBase
    {
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public DateTime Created
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Modified
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? Published
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