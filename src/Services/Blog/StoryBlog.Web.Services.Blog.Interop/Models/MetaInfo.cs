using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    /// <summary>
    /// 
    /// </summary>
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNavigation"></typeparam>
    [DataContract(IsReference = false, Name = "meta", Namespace = "http://storyblog.org/schemas/json/result/meta")]
    public class MetaInfo<TNavigation>
    {
        [JsonPropertyName("navigation")]
        public TNavigation Navigation
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

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TNavigation"></typeparam>
    /// <typeparam name="TResources"></typeparam>
    [DataContract(IsReference = false, Name = "meta", Namespace = "http://storyblog.org/schemas/json/result/meta")]
    public class MetaInfo<TNavigation, TResources>
    {
        [JsonPropertyName("navigation")]
        public TNavigation Navigation
        {
            get;
            set;
        }

        [JsonPropertyName("resources")]
        public TResources Resources
        {
            get;
            set;
        }
    }
}