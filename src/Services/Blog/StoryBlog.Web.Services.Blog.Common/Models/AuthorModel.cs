using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common.Models
{
    [DataContract]
    public sealed class AuthorModel
    {
        [DataMember]
        public long Id
        {
            get;
            set;
        }
    }
}