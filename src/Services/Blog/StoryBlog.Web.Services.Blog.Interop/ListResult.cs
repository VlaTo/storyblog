using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop
{
    [DataContract]
    public sealed class ListResultMetaInformation
    {
        [DataMember(Name = "navigation")]
        public Navigation Navigation
        {
            get;
            set;
        }
    }

    [DataContract]
    public class ListResult<TValue>
    {
        [DataMember(Name = "data")]
        public IEnumerable<TValue> Data
        {
            get;
            set;
        }

        [DataMember(Name = "meta")]
        public ListResultMetaInformation Meta
        {
            get;
            set;
        }
    }
}