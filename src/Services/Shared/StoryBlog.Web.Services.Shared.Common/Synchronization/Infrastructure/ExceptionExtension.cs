using System;
using System.Runtime.ExceptionServices;

namespace StoryBlog.Web.Services.Shared.Common.Synchronization.Infrastructure
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