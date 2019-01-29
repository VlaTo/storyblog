using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace StoryBlog.Web.Services.Identity.API.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class ApplySecurityHeadersAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            var result = context.Result;

            if (result is ViewResult)
            {
                ApplyHeaders(context.HttpContext.Response);
            }
            else
            {
                base.OnResultExecuting(context);
            }
        }

        private static void ApplyHeaders(HttpResponse response)
        {
            response.Headers.TryAdd("X-Content-Type-Options", "nosniff");
            response.Headers.TryAdd("X-Frame-Options", "SAMEORIGIN");

            const string csp = "default-src 'self';";
            // an example if you need client images to be displayed from twitter
            //var csp = "default-src 'self'; img-src 'self' https://pbs.twimg.com";

            // once for standards compliant browsers
            response.Headers.TryAdd("Content-Security-Policy", csp);

            // and once again for IE
            response.Headers.TryAdd("X-Content-Security-Policy", csp);
        }
    }
}