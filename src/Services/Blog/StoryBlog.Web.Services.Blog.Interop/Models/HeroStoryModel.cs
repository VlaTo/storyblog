using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://storyblog.org/schemas/json/models/hero")]
    public sealed class HeroStoryModel : StoryModelBase
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