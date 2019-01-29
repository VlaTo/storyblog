using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Domain.Models
{
    [DataContract(Name = "story")]
    public sealed class FeedStory
    {
        [DataMember(Name = "title")]
        public string Title
        {
            get;
            set;
        }

        [DataMember(Name = "slug")]
        public string Slug
        {
            get;
            set;
        }

        [DataMember(Name = "text")]
        public string Text
        {
            get;
            set;
        }
    }
}
