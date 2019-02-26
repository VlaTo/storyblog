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

        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "hero")]
        HeroPost = 4,

        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "featured")]
        FeaturedStories = 8
    }
}