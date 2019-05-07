using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Shared.Common
{
    /// <summary>
    /// Navigation tokens
    /// </summary>
    [DataContract(Name = "navigation")]
    public sealed class Navigation
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "prev")]
        public string Previous
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "next")]
        public string Next
        {
            get;
            set;
        }
    }
}