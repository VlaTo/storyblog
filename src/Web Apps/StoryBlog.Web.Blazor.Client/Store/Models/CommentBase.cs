using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    public abstract class CommentBase<TComment>
    {
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