using System;
using Microsoft.EntityFrameworkCore.Internal;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    public static class CommandResultExtension
    {
        public static bool IsSuccess(this CommandResult result)
        {
            if (null == result)
            {
                throw new ArgumentNullException(nameof(result));
            }

            return false == result.Exceptions.Any();
        }
    }
}