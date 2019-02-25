using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop
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