using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateCaptchaTokenAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ICaptcha service;
        private readonly ILogger logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="loggerFactory"></param>
        public ValidateCaptchaTokenAuthorizationFilter(ICaptcha service, ILoggerFactory loggerFactory)
        {
            this.service = service;
            logger = loggerFactory.CreateLogger(GetType());
        }

        /// <inheritdoc />
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var validation = service.ValidateRequest(context.HttpContext);

            if (validation.IsFailed)
            {
                logger.LogDebug($"Captcha validation failed, result: {validation.Result}");
                context.Result = new CaptchaValidationFailedResult(validation.Result);
            }

            return Task.CompletedTask;
        }
    }
}