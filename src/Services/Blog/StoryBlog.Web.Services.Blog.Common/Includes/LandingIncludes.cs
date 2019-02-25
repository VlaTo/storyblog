using System;
using StoryBlog.Web.Services.Shared.Common.Annotations;

namespace StoryBlog.Web.Services.Blog.Common.Includes
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum LandingIncludes
    {
        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "hero")]
        HeroPost = 1,

        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "featured")]
        FeaturedPosts = 2
    }
}