using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    public sealed class DefaultCaptchaStore : ICaptchaStore
    {
        private readonly CaptchaOptions options;
        private readonly IDictionary<BinaryBlob, GeneratedCaptcha> cache;

        public DefaultCaptchaStore(IOptions<CaptchaOptions> options)
        {
            this.options = options.Value;
            cache = new Dictionary<BinaryBlob, GeneratedCaptcha>();
        }

        public GeneratedCaptcha GetCaptcha(HttpContext context, CaptchaToken token)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            if (cache.TryGetValue(token.Value, out var captcha))
            {
                return captcha;
            }

            return null;
        }

        public void SetCaptcha(HttpContext context, GeneratedCaptcha captcha)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (null == captcha)
            {
                throw new ArgumentNullException(nameof(captcha));
            }

            var token = captcha.Token;

            cache[token.Value] = captcha;
        }

        public void InvalidateCaptcha(HttpContext context, CaptchaToken token)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (null == token)
            {
                throw new ArgumentNullException(nameof(token));
            }

            cache.Remove(token.Value);
        }
    }
}