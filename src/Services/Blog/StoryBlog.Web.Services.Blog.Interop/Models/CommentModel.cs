using System;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common.Models
{
    [DataContract]
    public sealed class CommentModel
    {
        [DataMember(Name = "id")]
        public long Id
        {
            get;
            set;
        }

        [DataMember(Name = "content")]
        public string Content
        {
            get;
            set;
        }

        [DataMember(Name = "author")]
        public AuthorModel Author
        {
            get;
            set;
        }

        [DataMember(Name = "created")]
        public DateTime Created
        {
            get;
            set;
        }

        [DataMember(Name = "modified")]
        public DateTime? Modified
        {
            get;
            set;
        }
    }
}