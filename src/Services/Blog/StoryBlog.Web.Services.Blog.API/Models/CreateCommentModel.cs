using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.API.Models
{
    [DataContract(Name = "comment")]
    public sealed class CreateCommentModel
    {
        [JsonPropertyName("content")]
        public string Content
        {
            get;
            set;
        }

        [JsonPropertyName("public")]
        public bool IsPublic
        {
            get;
            set;
        }
    }
}