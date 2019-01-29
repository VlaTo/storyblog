using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common
{
    [DataContract(Name = "navigation")]
    public sealed class Navigation
    {
        [DataMember(Name = "prev")]
        public string Previous
        {
            get;
            set;
        }

        [DataMember(Name = "next")]
        public string Next
        {
            get;
            set;
        }
    }
}