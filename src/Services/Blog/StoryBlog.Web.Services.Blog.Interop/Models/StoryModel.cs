using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop.Models
{
    [DataContract(Name = "story")]
    public sealed class StoryModel : StoryModelBase
    {
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