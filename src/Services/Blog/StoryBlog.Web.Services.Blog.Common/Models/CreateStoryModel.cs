using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common.Models
{
    [DataContract]
    public sealed class CreateStoryModel
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