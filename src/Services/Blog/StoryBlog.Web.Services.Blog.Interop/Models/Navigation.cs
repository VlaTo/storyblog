using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract(Name = "navigation")]
    public sealed class Navigation
    {
        [JsonPropertyName("prev")]
        public string Previous
        {
            get;
            set;
        }

        [JsonPropertyName("next")]
        public string Next
        {
            get;
            set;
        }
    }
}