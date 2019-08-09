using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
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
}