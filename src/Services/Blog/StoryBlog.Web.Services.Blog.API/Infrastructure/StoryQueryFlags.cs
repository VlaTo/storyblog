using StoryBlog.Web.Services.Blog.Infrastructure.Annotations;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class StoryQueryFlags
    {
        /// <summary>
        /// 
        /// </summary>
        [Key(Name = "author")]
        public bool IncludeAuthors
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [Key(Name = "comments")]
        public bool IncludeComments
        {
            get;
            set;
        }
    }
}