using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop
{
    [DataContract(Name = "result")]
    public class ListResult<TValue>
    {
        [DataMember(Name = "data")]
        public IEnumerable<TValue> Data
        {
            get;
            set;
        }

        [DataMember(Name = "meta")]
        public ResultMetaInformation Meta
        {
            get;
            set;
        }
    }
}