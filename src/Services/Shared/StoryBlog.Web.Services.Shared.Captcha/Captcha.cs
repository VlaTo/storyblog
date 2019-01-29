using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    public class Captcha : ICaptcha
    {
        private readonly ICaptchaImageGenerator generator;
        private readonly ICaptchaStore store;
        private readonly CaptchaOptions options;
        private readonly ILogger logger;
        private readonly Random random;

        public Captcha(
            IOptions<CaptchaOptions> snapshot,
            ICaptchaImageGenerator generator,
            ICaptchaStore store,
            ILoggerFactory loggerFactory)
        {
            this.generator = generator;
            this.store = store;
            options = snapshot.Value;

            logger = loggerFactory.CreateLogger(typeof(Captcha));
            random = new Random();
        }

        public void Create(HttpContext httpContext)
        {
            var feature = GetCaptchaFeature(httpContext);
            var captcha = GenerateCaptcha(options.AllowedChars, options.CaptchaLength);
        }

        public CaptchaValidationResult ValidateRequest(HttpContext httpContext)
        {
            return new CaptchaValidationResult();
        }

        public string GetImageUrl(HttpContext httpContext)
        {
            return null;
        }

        public static ICaptchaFeature GetCaptchaFeature(HttpContext context)
        {
            var feature = context.Features.Get<ICaptchaFeature>();

            if (null == feature)
            {
                feature = new DefaultCaptchaFeature();
                context.Features.Set(feature);
            }

            return feature;
        }

        private char[] GenerateCaptcha(string chars, int length)
        {
            var maxIndex = chars.Length;
            var buffer = new Span<char>(new char[length]);

            for (var count = 0; count < length; count++)
            {
                var index = random.Next(maxIndex);
                buffer[count] = chars[index];
            }

            return buffer.ToArray();
        }
    }
}