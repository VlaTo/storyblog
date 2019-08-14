using System.Text.Json.Serialization;

namespace StoryBlog.Web.Blazor.Client.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateCommentModel
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("content")]
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("public")]
        public bool IsPublic
        {
            get;
            set;
        }
    }
}