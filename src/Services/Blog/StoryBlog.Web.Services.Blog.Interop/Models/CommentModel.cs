using System;
using System.Text.Json.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CommentModel : CommentModelBase, IEquatable<CommentModel>
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("author")]
        public int Author
        {
            get;
            set;
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
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

        /// <inheritdoc cref="object.Equals(object)" />
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

        /// <inheritdoc cref="object.GetHashCode" />
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