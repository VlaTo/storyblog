using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://storyblog.org/schemas/json/models/comment")]
    public class CommentModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("id")]
        public long Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("parent")]
        public long? Parent
        {
            get;
            set;
        }

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
        [JsonPropertyName("created")]
        public DateTime Created
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("modified")]
        public DateTime? Modified
        {
            get;
            set;
        }
    }
}