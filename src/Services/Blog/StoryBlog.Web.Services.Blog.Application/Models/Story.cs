using System.Collections.Generic;

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
        public IList<Comment> Comments
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public Story(long id)
            : base(id)
        {
            Comments = new List<Comment>();
        }
    }
}