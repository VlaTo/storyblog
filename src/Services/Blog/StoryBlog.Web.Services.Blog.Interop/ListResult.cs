using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop
{
    [DataContract(IsReference = false, Name = "meta", Namespace = "http://storyblog.org/schemas/json/meta")]
    public sealed class ListResultMetaInformation
    {
        [DataMember(Name = "navigation")]
        public Navigation Navigation
        {
            get;
            set;
        }
    }

    [DataContract(IsReference = false, Name = "result", Namespace = "http://storyblog.org/schemas/json/result/list")]
    public sealed class ListResult<TValue>
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