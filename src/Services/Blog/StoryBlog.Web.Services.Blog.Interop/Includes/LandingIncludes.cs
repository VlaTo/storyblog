using System;
using StoryBlog.Web.Services.Blog.Interop.Core.Annotations;

namespace StoryBlog.Web.Services.Blog.Interop.Includes
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
        HeroStory = 1,

        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "featured")]
        FeaturedStories = 2,

        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "feed")]
        StoriesFeed = 4
    }
}