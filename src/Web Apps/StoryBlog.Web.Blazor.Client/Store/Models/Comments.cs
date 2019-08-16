using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Comment : CommentBase
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

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<CommentBase> Comments
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public Comment(Comment parent)
            : base(parent)
        {
            Comments = new CommentBase[0];
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PendingComment : CommentBase
    {
        public PendingComment(Comment parent)
            : base(parent)
        {
        }
    }
}