using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryModel
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
        public int AllCommentsCount
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
        public StoryModel()
        {
            Comments = new List<CommentModel>();
        }
    }
}