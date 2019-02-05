using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common.Models
{
    [DataContract(Name = "author")]
    public sealed class AuthorModel
    {
        [DataMember(Name = "id")]
        public long Id
        {
            get;
            set;
        }

        [DataMember(Name = "name")]
        public string UserName
        {
            get;
            set;
        }
    }
}