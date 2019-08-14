using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CommentBase
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
        public DateTimeOffset Published
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TComment"></typeparam>
    public abstract class CommentBase<TComment> : CommentBase
    {
        /// <summary>
        /// 
        /// </summary>
        public TComment Parent
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<TComment> Comments
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        protected CommentBase(TComment parent)
        {
            Parent = parent;
            Comments = new Collection<TComment>();
        }
    }
}