using System;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract]
    public class StoryModelBase
    {
        [DataMember(Name = "id")]
        public long Id
        {
            get;
            set;
        }

        [DataMember(Name = "title")]
        public string Title
        {
            get;
            set;
        }

        [DataMember(Name = "slug")]
        public string Slug
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

        [DataMember(Name = "public")]
        public bool IsPublic
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