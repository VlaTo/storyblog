using System;

namespace StoryBlog.Web.Services.Blog.Interop.Core.Annotations
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