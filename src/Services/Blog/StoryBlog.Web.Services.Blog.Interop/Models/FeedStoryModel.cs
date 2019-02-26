using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract(Name = "story")]
    public sealed class FeedStoryModel : StoryModelBase
    {
        [DataMember(Name = "comments")]
        public uint CommentsCount
        {
            get;
            set;
        }
    }
}