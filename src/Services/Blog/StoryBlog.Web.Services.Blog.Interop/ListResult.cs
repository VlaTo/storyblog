using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.Interop
{
    [DataContract(IsReference = false,Name = "meta", Namespace = "http://storyblog.org/schemas/json/result/meta")]
    public abstract class ListResultMeta
    {
        [DataMember(Name = "navigation")]
        public Navigation Navigation
        {
            get;
            set;
        }
    }

    [DataContract(IsReference = false, Name = "resources", Namespace = "")]
    public abstract class ListResultResources
    {
    }

    [DataContract(IsReference = false, Name = "meta", Namespace = "http://storyblog.org/schemas/json/result/resources")]
    public sealed class ResourcesMeta : ListResultMeta
    {
        [DataMember(Name = "resources")]
        public ListResultResources Resources
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TMeta"></typeparam>
    [DataContract(IsReference = false, Name = "result", Namespace = "http://storyblog.org/schemas/json/result/list")]
    public class ListResult<TValue, TMeta>
        where TMeta : ListResultMeta
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "data")]
        public IEnumerable<TValue> Data
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "meta")]
        public TMeta Meta
        {
            get;
            set;
        }
    }
}