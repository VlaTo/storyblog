using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common.Models
{
    [DataContract(Name = "comment")]
    public sealed class CreateCommentModel
    {
        [DataMember(Name = "content")]
        public string Content
        {
            get;
            set;
        }

        [DataMember(Name = "public")]
        public bool IsPublic
        {
            get;
            set;
        }
    }
}