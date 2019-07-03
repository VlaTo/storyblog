using System.Runtime.Serialization;

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
    public sealed class ResourcesMetaInfo<TResources> : ResultMetaInfo
        where TResources : ResultResources
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "resources")]
        public TResources Resources
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [DataContract(IsReference = false, Name = "meta", Namespace = "http://storyblog.org/schemas/json/result/resources")]
    public sealed class ResourcesNavigationMetaInfo<TResources> : NavigationMetaInfo
        where TResources : ResultResources
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember(Name = "resources")]
        public TResources Resources
        {
            get;
            set;
        }
    }
}