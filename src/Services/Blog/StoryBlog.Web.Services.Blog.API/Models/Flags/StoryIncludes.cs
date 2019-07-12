using System;
using StoryBlog.Web.Services.Blog.API.Core.Annotations;

namespace StoryBlog.Web.Services.Blog.API.Models.Flags
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