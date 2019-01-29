using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Shared.Captcha;

namespace StoryBlog.Web.Services.Identity.API.Infrastructure
{
    public class ValidateCaptchaTokenAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly ICaptcha service;
        private readonly ILogger logger;

        public ValidateCaptchaTokenAuthorizationFilter(ICaptcha service, ILoggerFactory loggerFactory)
        {
            this.service = service;
            logger = loggerFactory.CreateLogger(GetType());
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}