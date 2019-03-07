using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultCaptchaFeature : ICaptchaFeature
    {
        private readonly HttpContext context;
        private readonly CaptchaOptions options;

        /// <inheritdoc />
        public bool HasCaptchaToken => context.Request.Cookies.ContainsKey(options.Cookie.Name);

        /// <inheritdoc />
        public CaptchaToken CaptchaToken
        {
            get
            {
                var name = options.Cookie.Name;
                var value = context.Request.Cookies[name];
                var bytes = WebEncoders.Base64UrlDecode(value).AsMemory();
                var blob = new BinaryBlob(256, bytes);

                return new CaptchaToken(blob);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DefaultCaptchaFeature(HttpContext context)
        {
            var snapshot = (IOptions<CaptchaOptions>) context.RequestServices.GetService(typeof(IOptions<CaptchaOptions>));

            this.context = context;
            options = snapshot.Value;
        }
    }
}