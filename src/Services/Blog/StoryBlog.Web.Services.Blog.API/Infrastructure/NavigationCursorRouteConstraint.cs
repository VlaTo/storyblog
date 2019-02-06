using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NavigationCursorRouteConstraint : IRouteConstraint
    {
        /// <inheritdoc cref="IRouteConstraint.Match" />
        public  bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            switch (routeDirection)
            {
                case RouteDirection.IncomingRequest:
                {
                    if (false == values.ContainsKey(routeKey))
                    {
                        return false;
                    }

                    var value = values[routeKey];

                    return NavigationCursorEncoder.TryParse(Convert.ToString(value), out var cursor) && null != cursor;
                }

                case RouteDirection.UrlGeneration:
                {
                    if (false == values.ContainsKey(routeKey))
                    {
                        return false;
                    }

                    return values[routeKey] is NavigationCursor;
                }
            }

            return false;
        }
    }
}