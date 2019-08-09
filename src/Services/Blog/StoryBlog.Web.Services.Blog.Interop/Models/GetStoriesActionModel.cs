using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract(IsReference = false, Name = "result", Namespace = "http://storyblog.org/schemas/json/result/list")]
    public sealed class GetStoriesActionModel
    {
        [JsonPropertyName("data")]
        public IEnumerable<StoryModel> Data
        {
            get;
            set;
        }

        [JsonPropertyName("meta")]
        public MetaInfo<Navigation> Meta
        {
            get;
            set;
        }
    }
}