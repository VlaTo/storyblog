using System;
using System.Runtime.ExceptionServices;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Core
{
    internal static class ExceptionExtension
    {
        internal static Exception PrepareForRethrow(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
            return exception;
        }
    }
}