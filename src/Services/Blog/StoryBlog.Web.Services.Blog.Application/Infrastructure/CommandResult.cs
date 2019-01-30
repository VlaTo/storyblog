using System;
using System.Collections.Generic;
using System.Linq;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandResult
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Exception> Exceptions
        {
            get;
        }

        protected CommandResult()
        {
            Exceptions = Enumerable.Empty<Exception>();
        }

        protected CommandResult(IEnumerable<Exception> exceptions)
        {
            Exceptions = exceptions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        public static CommandResult Error(params Exception[] exceptions)
        {
            if (null == exceptions)
            {
                throw new ArgumentNullException(nameof(exceptions));
            }

            return new CommandResult(exceptions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static CommandResult Success()
        {
            return new CommandResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        public static CommandResult<TResult> Success<TResult>(TResult result)
        {
            return new CommandResult<TResult>(result);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public sealed class CommandResult<TData> : CommandResult
    {
        public TData Data
        {
            get;
        }

        internal CommandResult(TData data)
        {
            Data = data;
        }
    }
}