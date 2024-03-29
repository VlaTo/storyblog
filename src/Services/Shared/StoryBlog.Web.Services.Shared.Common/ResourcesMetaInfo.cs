﻿using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Shared.Common
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(IsReference = false, Name = "resources", Namespace = "")]
    public abstract class ResultResources
    {
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(IsReference = false, Name = "meta", Namespace = "http://storyblog.org/schemas/json/result/resources")]
    public sealed class ResourcesMetaInfo : NavigationMetaInfo
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "resources")]
        public ResultResources Resources
        {
            get;
            set;
        }
    }
}