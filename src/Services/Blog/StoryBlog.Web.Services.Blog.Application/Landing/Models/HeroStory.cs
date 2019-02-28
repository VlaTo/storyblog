using StoryBlog.Web.Services.Blog.Application.Stories.Models;

namespace StoryBlog.Web.Services.Blog.Application.Landing.Models
{
    public sealed class HeroStory : StoryBase
    {
        public int CommentsCount
        {
            get;
            set;
        }

        public HeroStory(long id)
            : base(id)
        {
        }
    }
}