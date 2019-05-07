using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Shared.Common
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract(IsReference = false, Name = "meta", Namespace = "http://storyblog.org/schemas/json/result/meta")]
    public abstract class ResultMetaInfo
    {
    }
}