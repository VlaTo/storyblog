﻿using System;
using System.Security.Policy;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    public class DefaultCaptcha : ICaptcha
    {
        private readonly ICaptchaStore store;
        private readonly CaptchaOptions options;
        private readonly ILogger logger;
        private readonly CaptchaTextGenerator textGenerator;

        public DefaultCaptcha(
            IOptions<CaptchaOptions> options,
            ICaptchaStore store,
            ILoggerFactory loggerFactory)
        {
            this.store = store;
            this.options = options.Value;

            logger = loggerFactory.CreateLogger(typeof(DefaultCaptcha));
            textGenerator = new CaptchaTextGenerator(this.options.AllowedChars, this.options.CaptchaLength);
        }

        public void Create(HttpContext context)
        {
            var feature = GetCaptchaFeature(context);

            if (feature.HasCaptchaToken)
            {
                store.InvalidateCaptcha(context, feature.CaptchaToken);
            }

            var text = textGenerator.Generate();
            var blob = new BinaryBlob(256);
            var token = new CaptchaToken(blob);
            var cookie = options.Cookie.Build(context);
            var captcha = new GeneratedCaptcha(token, text, DateTime.UtcNow);

            store.SetCaptcha(context, captcha);

            context.Response.Cookies.Append(
                options.Cookie.Name,
                Encoding.UTF8.GetString(token.Value.GetData()),
                cookie
            );

            logger.LogDebug($"New CAPTCHA was generated for token: {blob.DebugString}");
        }

        public CaptchaValidationResult ValidateRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public string GetImageUrl(HttpContext context)
        {
            //var feature = GetCaptchaFeature(context);

            //if (false == feature.HasCaptchaToken)
            //{
            //    return null;
            //}

            //var blob = feature.CaptchaToken.Value;
            //var token = Convert.ToBase64String(blob.GetData());
            var path = options.RequestPath;// + '/' + UrlEncoder.Default.Encode(token);

            return path;
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
    }
}