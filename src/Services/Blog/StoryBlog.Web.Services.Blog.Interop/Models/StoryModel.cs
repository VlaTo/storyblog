using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(Namespace = "http://storyblog.org/schemas/json/models/story")]
    public sealed class StoryModel : StoryModelBase
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "comments")]
        public IEnumerable<CommentModel> Comments
        {
            get;
            set;
        }

        public StoryModel()
        {
            Comments = Enumerable.Empty<CommentModel>();
        }
    }
}