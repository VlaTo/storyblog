using System.Collections.Generic;
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

    [DataContract(IsReference = false, Name = "resources", Namespace = "http://astoryblog.org/schemas/json/resources")]
    public sealed class StoryResources
    {
        [JsonPropertyName("authors")]
        public AuthorModel[] Authors
        {
            get;
            set;
        }
    }

    [DataContract(IsReference = false, Name = "meta", Namespace = "http://storyblog.org/schemas/json/result/meta")]
    public class MetaInfo
    {
        [JsonPropertyName("navigation")]
        public Navigation Navigation
        {
            get;
            set;
        }

        [JsonPropertyName("resources")]
        public StoryResources Resources
        {
            get;
            set;
        }
    }

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
        public MetaInfo Meta
        {
            get;
            set;
        }
    }
}