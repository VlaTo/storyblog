using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract(IsReference = false, Name = "result", Namespace = "http://storyblog.org/schemas/json/result/story")]
    public sealed class GetStoryActionModel
    {
        [JsonPropertyName("data")]
        public StoryModel Data
        {
            get;
            set;
        }

        [JsonPropertyName("meta")]
        public MetaInfo Meta
        {
            get;
            set;
        }
    }
}