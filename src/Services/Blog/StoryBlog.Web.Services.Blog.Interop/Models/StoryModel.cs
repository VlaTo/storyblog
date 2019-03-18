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
        [DataMember(Name = "closed")]
        public bool Closed
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "comments")]
        //public IReadOnlyCollection<CommentModel> Comments
        public CommentModel[] Comments
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public StoryModel()
        {
            //Comments = Enumerable.Empty<CommentModel>();
            Comments = new CommentModel[0];
        }
    }
}