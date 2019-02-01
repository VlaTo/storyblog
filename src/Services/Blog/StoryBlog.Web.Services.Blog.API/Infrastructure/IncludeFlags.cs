using StoryBlog.Web.Services.Blog.Infrastructure.Annotations;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    public sealed class IncludeFlags
    {
        [Key(Name = "author")]
        public bool IncludeAuthors
        {
            get;
            set;
        }

        [Key(Name = "comments")]
        public bool IncludeComments
        {
            get;
            set;
        }
    }
}