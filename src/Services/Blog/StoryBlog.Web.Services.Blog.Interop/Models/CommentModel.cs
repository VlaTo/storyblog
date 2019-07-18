﻿using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract(Namespace = "http://storyblog.org/schemas/json/models/comment")]
    public sealed class CommentModel : IEquatable<CommentModel>
    {
        [JsonPropertyName("id")]
        public long Id
        {
            get;
            set;
        }

        [JsonPropertyName("parent")]
        public long? Parent
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

        [JsonPropertyName("modified")]
        public DateTime? Modified
        {
            get;
            set;
        }

        public bool Equals(CommentModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id
                   && String.Equals(Content, other.Content)
                   && Author.Equals(other.Author)
                   && Created.Equals(other.Created)
                   && Modified.Equals(other.Modified);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return obj is CommentModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();

                hashCode = (hashCode * 397) ^ (Content != null ? Content.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Author.GetHashCode();
                hashCode = (hashCode * 397) ^ Created.GetHashCode();
                hashCode = (hashCode * 397) ^ Modified.GetHashCode();

                return hashCode;
            }
        }
    }
}