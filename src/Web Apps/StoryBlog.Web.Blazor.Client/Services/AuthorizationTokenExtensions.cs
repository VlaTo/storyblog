using System;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal static class AuthorizationTokenExtensions
    {
        public static string ToString(this AuthorizationToken token)
        {
            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            return token.Scheme + ' ' + token.Payload;
        }
    }
}