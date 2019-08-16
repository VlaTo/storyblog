using System;

namespace StoryBlog.Web.Blazor.Client.Store.Models.Data
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentCreatedResult
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public long? Parent
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
        public string Content
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
    }
}