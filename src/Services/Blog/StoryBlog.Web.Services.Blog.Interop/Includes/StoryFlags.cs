using System;
using System.ComponentModel;
using StoryBlog.Web.Services.Blog.Interop.Core.Annotations;
using StoryBlog.Web.Services.Blog.Interop.Core.Converters;

namespace StoryBlog.Web.Services.Blog.Interop.Includes
{
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(StoryFlagsConverter))]
    [Flags]
    public enum StoryFlags
    {
        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "authors")]
        Authors = 1,

        /// <summary>
        /// 
        /// </summary>
        [Flag(Key = "comments")]
        Comments = 2
    }
}