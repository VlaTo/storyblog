using System;
using System.Collections.Generic;
using System.Linq;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    public class CommandResult
    {
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

        public static CommandResult Error(params Exception[] exceptions)
        {
            if (null == exceptions)
            {
                throw new ArgumentNullException(nameof(exceptions));
            }

            return new CommandResult(exceptions);
        }

        public static CommandResult Ok()
        {
            return new CommandResult();
        }

        public static CommandResult<TResult> Ok<TResult>(TResult result)
        {
            return new CommandResult<TResult>(result);
        }
    }

    public sealed class CommandResult<TResult> : CommandResult
    {
        public TResult Result
        {
            get;
        }

        internal CommandResult(TResult result)
        {
            Result = result;
        }
    }
}