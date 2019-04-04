using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentModel
    {
        /// <summary>
        /// 
        /// </summary>
        public long Id
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public CommentModel Parent
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public AuthorModel Author
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
        public IList<CommentModel> Comments
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="id"></param>
        public CommentModel(CommentModel parent, long id)
        {
            Parent = parent;
            Id = id;
            Comments = new List<CommentModel>();
        }
    }
}