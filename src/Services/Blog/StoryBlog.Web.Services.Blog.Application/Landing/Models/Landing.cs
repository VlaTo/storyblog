using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StoryBlog.Web.Services.Blog.Application.Landing.Models
{
    public sealed class Landing
    {
        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public Story HeroStory
        {
            get;
            set;
        }

        public ICollection<Story> FeaturedStories
        {
            get;
        }

        public Landing()
        {
            FeaturedStories = new Collection<Story>();
        }
    }
}
