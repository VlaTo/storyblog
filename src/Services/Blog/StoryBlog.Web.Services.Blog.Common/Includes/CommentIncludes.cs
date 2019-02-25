using System;
using StoryBlog.Web.Services.Shared.Common.Annotations;

namespace StoryBlog.Web.Services.Blog.Common.Includes
{
    [Flags]
    public enum CommentIncludes
    {
        /// <summary>   
        /// 
        /// </summary>
        [Flag(Key = "author")]
        Authors = 1
    }
}