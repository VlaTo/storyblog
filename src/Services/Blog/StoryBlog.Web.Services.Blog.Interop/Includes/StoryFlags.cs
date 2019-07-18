using System;
using StoryBlog.Web.Services.Blog.Interop.Core.Annotations;

namespace StoryBlog.Web.Services.Blog.Interop.Includes
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum StoryFlags
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
        Comments = 2
    }
}