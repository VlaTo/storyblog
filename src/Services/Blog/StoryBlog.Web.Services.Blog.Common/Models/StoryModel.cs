using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common.Models
{
    [DataContract]
    public sealed class StoryModel
    {
        [DataMember]
        public long Id
        {
            get;
            set;
        }

        [DataMember]
        public string Title
        {
            get;
            set;
        }

        [DataMember]
        public string Content
        {
            get;
            set;
        }

        [DataMember]
        public AuthorModel Author
        {
            get;
            set;
        }
    }
}
