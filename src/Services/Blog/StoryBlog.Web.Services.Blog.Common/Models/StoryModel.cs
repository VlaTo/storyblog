using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Common.Models
{
    [DataContract(Name = "story")]
    public sealed class StoryModel
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

        [DataMember(Name = "comments")]
        public ICollection<CommentModel> Comments
        {
            get;
        }

        public StoryModel()
        {
            Comments = new Collection<CommentModel>();
        }
    }
}
