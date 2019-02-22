using System;
using StoryBlog.Web.Services.Shared.Common.Annotations;

namespace StoryBlog.Web.Services.Blog.Common.Includes
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
        Authors,

        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "comments")]
        Comments
    }
}