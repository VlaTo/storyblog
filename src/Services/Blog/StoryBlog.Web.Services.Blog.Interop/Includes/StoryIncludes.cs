using System;
using StoryBlog.Web.Services.Shared.Common.Annotations;

namespace StoryBlog.Web.Services.Blog.Interop.Includes
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum StoryIncludes
    {
        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "author")]
        Authors = 1,

        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "comments")]
        Comments = 2,
    }
}