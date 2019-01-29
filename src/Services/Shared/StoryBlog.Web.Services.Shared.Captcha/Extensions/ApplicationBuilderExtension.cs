using System;
using Microsoft.AspNetCore.Builder;

namespace StoryBlog.Web.Services.Shared.Captcha.Extensions
{
    public static class ApplicationBuilderExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCaptcha(this IApplicationBuilder app)
        {
            if (null == app)
            {
                throw new ArgumentNullException(nameof(app));
            }

            return app.UseMiddleware<DefaultCaptchaMiddleware>();
        }
    }
}