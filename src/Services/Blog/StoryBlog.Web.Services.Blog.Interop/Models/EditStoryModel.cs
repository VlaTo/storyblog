using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract]
    public sealed class EditStoryModel
    {
        [Required]
        [DataType(DataType.Text)]
        [DataMember(Name = "title")]
        public string Title
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.MultilineText)]
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