using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentCreatedModel : CommentModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("author")]
        public AuthorModel Author
        {
            get;
            set;
        }
    }
}