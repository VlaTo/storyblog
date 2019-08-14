using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Story : StoryBase
    {
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
        /// Gets or sets all comments count.
        /// </summary>
        public int CommentsCount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Story()
        {
            Comments = new List<Comment>();
        }
    }
}