using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract(Namespace = "http://storyblog.org/schemas/json/models/story")]
    public sealed class StoryModel
    {
        [JsonPropertyName("title")]
        public string Title
        {
            get;
            set;
        }

        [JsonPropertyName("slug")]
        public string Slug
        {
            get;
            set;
        }

        [JsonPropertyName("content")]
        public string Content
        {
            get;
            set;
        }

        [JsonPropertyName("closed")]
        public bool Closed
        {
            get;
            set;
        }

        [JsonPropertyName("author")]
        public int Author
        {
            get;
            set;
        }

        [JsonPropertyName("created")]
        public DateTime Created
        {
            get;
            set;
        }

        [JsonPropertyName("published")]
        public DateTime? Published
        {
            get;
            set;
        }

        [JsonPropertyName("comments")]
        public ICollection<CommentModel> Comments
        {
            get;
            set;
        }

        public StoryModel()
        {
            Comments = new Collection<CommentModel>();
        }
    }
}