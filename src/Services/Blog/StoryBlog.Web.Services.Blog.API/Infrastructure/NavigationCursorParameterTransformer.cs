using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using StoryBlog.Web.Services.Blog.Application.Models;

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

            Debug.WriteLine($"[NavigationCursorParameterTransformer.TransformOutbound] Value: {value}");

            return Convert.ToString(value);
        }
    }
}