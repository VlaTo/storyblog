using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryBlog.Web.Services.Blog.Application.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Story : StoryBase
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
        public ICollection<Comment> Comments
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
        /// <param name="id"></param>
        public Story(long id)
            : base(id)
        {
            Comments = new Collection<Comment>();
        }
}
}