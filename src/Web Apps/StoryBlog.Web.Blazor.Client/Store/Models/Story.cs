using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.Store.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Story : StoryBase
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

        public Story()
        {
            Comments = new List<Comment>();
        }
    }
}