using StoryBlog.Web.Services.Blog.Infrastructure.Annotations;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    internal sealed class CommentQueryFlags
    {
        /// <summary>
        /// 
        /// </summary>
        [Key(Name = "author")]
        public bool IncludeAuthor
        {
            get;
            set;
        }
    }
}