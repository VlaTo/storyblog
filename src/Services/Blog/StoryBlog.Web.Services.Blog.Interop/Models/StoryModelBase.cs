using System;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://storyblog.org/schemas/json/models/base/story")]
    public abstract class StoryModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "title")]
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "slug")]
        public string Slug
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "content")]
        public string Content
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "author")]
        public int Author
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "created")]
        public DateTime Created
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "published")]
        public DateTime? Published
        {
            get;
            set;
        }
    }
}