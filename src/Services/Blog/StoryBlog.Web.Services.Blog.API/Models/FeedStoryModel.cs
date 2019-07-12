using System.Runtime.Serialization;
using StoryBlog.Web.Services.Blog.Interop.Models;

namespace StoryBlog.Web.Services.Blog.API.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://storyblog.org/schemas/json/models/feed/story")]
    public sealed class FeedStoryModel : StoryModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "comments")]
        public int Comments
        {
            get;
            set;
        }
    }
}