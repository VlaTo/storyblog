using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace StoryBlog.Web.Services.Shared.Common
{
    [DataContract(IsReference = false, Name = "result", Namespace = "http://storyblog.org/schemas/json/result/unary")]
    public class Result<TValue, TMetaInfo>
        where TMetaInfo : ResultMetaInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "data")]
        public TValue Data
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
