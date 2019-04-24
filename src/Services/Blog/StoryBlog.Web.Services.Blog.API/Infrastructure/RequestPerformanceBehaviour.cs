using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private static readonly TimeSpan Critical = TimeSpan.FromMilliseconds(500.0d);

        private readonly ILogger<TRequest> logger;
        private readonly Stopwatch stopwatch;

        public RequestPerformanceBehaviour(ILogger<TRequest> logger)
        {
            this.logger = logger;
            stopwatch = new Stopwatch();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;

            try
            {
                stopwatch.Start();
                response = await next.Invoke();
            }
            finally
            {
                stopwatch.Stop();

                if (stopwatch.Elapsed > Critical)
                {
                    var name = typeof(TRequest).Name;
                    logger.LogWarning("Request: {Name} takes {Elapsed} {@Request}", name, stopwatch.Elapsed, request);
                }
            }

            return response;
        }
    }
}