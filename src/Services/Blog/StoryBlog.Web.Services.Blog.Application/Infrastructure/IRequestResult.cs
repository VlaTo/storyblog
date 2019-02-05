using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRequestResult
    {
        /// <summary>
        /// 
        /// </summary>
        IEnumerable<Exception> Exceptions
        {
            get;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IRequestResult<out TData> : IRequestResult
    {
        /// <summary>
        /// 
        /// </summary>
        TData Data
        {
            get;
        }
    }
}