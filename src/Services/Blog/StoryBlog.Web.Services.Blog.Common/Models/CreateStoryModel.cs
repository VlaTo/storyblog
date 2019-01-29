using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common.Models
{
    [DataContract]
    public sealed class CreateStoryModel
    {
        [DataMember]
        public string Title
        {
            get;
            set;
        }
    }
}