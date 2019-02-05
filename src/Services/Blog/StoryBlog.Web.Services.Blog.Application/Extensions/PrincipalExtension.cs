using System;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Extensions
{
    internal static class PrincipalExtension
    {
        public static long GetId(this IPrincipal principal)
        {
            if (null == principal)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            var identity = principal.Identity;

            if (null != identity&&identity.IsAuthenticated)
            {
                return 1;
            }

            return -1;
        }
    }
}