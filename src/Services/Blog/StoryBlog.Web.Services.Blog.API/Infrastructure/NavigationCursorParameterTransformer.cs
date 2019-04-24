using Microsoft.AspNetCore.Routing;
using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using System;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NavigationCursorParameterTransformer : IOutboundParameterTransformer
    {
        /// <inheritdoc cref="IOutboundParameterTransformer.TransformOutbound" />
        public string TransformOutbound(object value)
        {
            if (value is NavigationCursor cursor)
            {
                var token = NavigationCursorEncoder.ToEncodedString(cursor);
                return token;
            }

            return Convert.ToString(value);
        }
    }
}