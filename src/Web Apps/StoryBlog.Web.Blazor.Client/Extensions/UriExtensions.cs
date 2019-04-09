using System;

namespace StoryBlog.Web.Blazor.Client.Extensions
{
    internal static class UriBuilderExtensions
    {
        public static string SchemeWithAuthority(this Uri source)
        {
            var builder = new UriBuilder
            {
                Scheme = source.Scheme,
                Host = source.Host,
                Port = source.Port
            };
            return builder.ToString();
        }
    }
}