using System.Collections.Generic;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Shared.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TMetaInfo"></typeparam>
    [DataContract(IsReference = false, Name = "result", Namespace = "http://storyblog.org/schemas/json/result/list")]
    public class ListResult<TValue, TMetaInfo>
        where TMetaInfo : ResultMetaInfo
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
        public TMetaInfo Meta
        {
            get;
            set;
        }
    }
}