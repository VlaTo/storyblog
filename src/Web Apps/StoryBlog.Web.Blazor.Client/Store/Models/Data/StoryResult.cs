using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Client.Store.Models.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Slug
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Author Author
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Published
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsCommentsClosed
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Comment> Comments
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public StoryResult()
        {
            Comments = new List<Comment>();
        }
    }
}