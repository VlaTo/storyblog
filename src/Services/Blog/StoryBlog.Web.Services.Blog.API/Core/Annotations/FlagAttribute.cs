using System;

namespace StoryBlog.Web.Services.Blog.API.Core.Annotations
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class FlagAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key
        {
            get;
            set;
        }
    }
}