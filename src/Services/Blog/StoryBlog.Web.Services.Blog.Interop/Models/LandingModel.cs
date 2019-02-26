using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract]
    public sealed class LandingModel
    {
        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "title")]
        public string Title
        {
            get;
            set;
        }

        [DataType(DataType.MultilineText)]
        [DataMember(Name = "description")]
        public string Description
        {
            get;
            set;
        }

        [DataMember(Name = "hero")]
        public FeedStoryModel HeroStory
        {
            get;
            set;
        }

        [DataMember(Name = "featured")]
        public IEnumerable<FeedStoryModel> FeaturedStories
        {
            get;
            set;
        }

        [DataMember(Name = "feed")]
        public IEnumerable<FeedStoryModel> StoriesFeed
        {
            get;
            set;
        }

        public LandingModel()
        {
            FeaturedStories = Enumerable.Empty<FeedStoryModel>();
            StoriesFeed = Enumerable.Empty<FeedStoryModel>();
        }
    }
}