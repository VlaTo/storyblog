using System.Collections.Specialized;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Domain.Exceptions;
using System.Net;
using System.Runtime.Serialization;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment environment;
        private readonly ILogger<HttpGlobalExceptionFilter> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="logger"></param>
        public HttpGlobalExceptionFilter(
            IHostingEnvironment environment,
            ILogger<HttpGlobalExceptionFilter> logger
        )
        {
            this.environment = environment;
            this.logger = logger;
        }

        /// <inheritdoc cref="IExceptionFilter.OnException" />
        public void OnException(ExceptionContext context)
        {
            logger.LogError(
                new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message
            );

            if (context.Exception is DomainException)
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] { context.Exception.Message }
                };

                context.Result = new BadRequestObjectResult(json);
                //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                var json = new JsonErrorResponse
                {
                    Messages = new[] { "An error ocurr.Try it again." }
                };

                if (environment.IsDevelopment())
                {
                    json.Exception = context.Exception;
                }

                context.Result = new ObjectResult(json);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            context.ExceptionHandled = true;
        }

        /// <summary>
        /// 
        /// </summary>
        [DataContract]
        private class JsonErrorResponse
        {
            [DataMember(Name = "messages")]
            public string[] Messages
            {
                get;
                set;
            }

            [DataMember(Name = "exception")]
            public object Exception
            {
                get;
                set;
            }
        }
    }
}