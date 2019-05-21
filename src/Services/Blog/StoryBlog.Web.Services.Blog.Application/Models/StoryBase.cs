using System;

namespace StoryBlog.Web.Services.Blog.Application.Models
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
        public DateTimeOffset Created
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? Modified
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTimeOffset? Published
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