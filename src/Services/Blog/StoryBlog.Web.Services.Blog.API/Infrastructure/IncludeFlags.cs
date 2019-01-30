using StoryBlog.Web.Services.Blog.Infrastructure;
using StoryBlog.Web.Services.Blog.Infrastructure.Annotations;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    public sealed class IncludeFlags : FlagParser
    {
        protected override char Separator => ',';

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