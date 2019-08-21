using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// Комментарий пользователя
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
        public Comment(string storySlug, Comment parent)
            : base(storySlug, parent)
        {
            Comments = new CommentBase[0];
        }
    }

    /// <summary>
    /// Комментарий на рассмотрении
    /// </summary>
    public sealed class PendingComment : CommentBase
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Reference
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="reference"></param>
        public PendingComment(string storySlug, Comment parent, Guid reference)
            : base(storySlug, parent)
        {
            Reference = reference;
        }
    }

    /// <summary>
    /// Комментарий создается
    /// </summary>
    public sealed class ComposeComment : CommentBase
    {
        public Guid Reference
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="reference"></param>
        public ComposeComment(string storySlug, Comment parent, Guid reference)
            : base(storySlug, parent)
        {
            Reference = reference;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SavingComment : CommentBase
    {
        public Guid Reference
        {
            get;
        }

        public SavingComment(string storySlug, Comment parent, Guid reference)
            : base(storySlug, parent)
        {
            Reference = reference;
        }
    }
}