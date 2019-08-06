using System;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.API.Models
{
    [DataContract(Namespace = "http://storyblog.org/schemas/json/models/author")]
    public sealed class AuthorModel : IEquatable<AuthorModel>
    {
        [DataMember(Name = "name")]
        public string Name
        {
            get;
            set;
        }

        public bool Equals(AuthorModel other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            //return Id == other.Id && string.Equals(Name, other.Name);
            return String.Equals(Name, other.Name);
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

            return obj is AuthorModel other && Equals(other);
        }

        public override int GetHashCode()
        {
            var name = Name;

            unchecked
            {
                //return (Id.GetHashCode() * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return (null != name ? name.GetHashCode() : 0) ^ 397;
            }
        }
    }
}