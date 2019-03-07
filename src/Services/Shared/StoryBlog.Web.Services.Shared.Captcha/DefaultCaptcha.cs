using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultCaptcha : ICaptcha
    {
        private readonly ICaptchaStore store;
        private readonly CaptchaOptions options;
        private readonly ILogger logger;
        private readonly CaptchaTextGenerator textGenerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="store"></param>
        /// <param name="loggerFactory"></param>
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

        /// <inheritdoc cref="ICaptcha.Create" />
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
                WebEncoders.Base64UrlEncode(token.Value.GetData()),
                cookie
            );

            logger.LogDebug($"New CAPTCHA was generated: {options.Cookie.Name}={blob.DebugString}");
        }

        /// <inheritdoc cref="ICaptcha.ValidateRequest" />
        public CaptchaValidationResult ValidateRequest(HttpContext context)
        {
            var feature = GetCaptchaFeature(context);

            if (false == feature.HasCaptchaToken)
            {
                return new CaptchaValidationResult(ValidationResult.NotExists);
            }

            var token = feature.CaptchaToken;
            var captcha = store.GetCaptcha(context, token);

            if (false == NotExpired(captcha, options.Timeout))
            {
                return new CaptchaValidationResult(ValidationResult.Expired);
            }

            if (context.Request.HasFormContentType)
            {
                if (context.Request.Form.TryGetValue(options.FormField.Name, out var field) && 1 == field.Count)
                {
                    var comparer = CreateComparer(options.Comparison);
                    var value = field[0].ToCharArray();

                    if (comparer.Equals(captcha.Captcha, value))
                    {
                        return new CaptchaValidationResult(ValidationResult.Success);
                    }
                }

                return new CaptchaValidationResult(ValidationResult.Mismatch);
            }

            return new CaptchaValidationResult(ValidationResult.NotExists);
        }

        /// <inheritdoc cref="ICaptcha.GetImageUrl" />
        public string GetImageUrl(HttpContext context)
        {
            var feature = GetCaptchaFeature(context);

            if (false == feature.HasCaptchaToken)
            {
            //    return null;
            }

            return options.RequestPath;
        }

        /// <inheritdoc />
        public FormFieldDescription GetFormFieldDescription()
        {
            return new FormFieldDescription(options.FormField.Name, "Введите код с картинки");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ICaptchaFeature GetCaptchaFeature(HttpContext context)
        {
            var feature = context.Features.Get<ICaptchaFeature>();

            if (null == feature)
            {
                feature = new DefaultCaptchaFeature(context);
                context.Features.Set(feature);
            }

            return feature;
        }

        internal static bool NotExpired(GeneratedCaptcha captcha, TimeSpan timeout)
        {
            var duration = DateTime.UtcNow - captcha.CreatedAt;
            return duration <= timeout;
        }

        private static CaptchaComparer CreateComparer(CaptchaComparisonMode mode)
        {
            switch (mode)
            {
                case CaptchaComparisonMode.CaseInsensitive:
                {
                    return CaptchaComparer.OrdinalIgnoreCase;
                }

                case CaptchaComparisonMode.CaseSensitive:
                {
                    return CaptchaComparer.Ordinal;
                }

                default:
                {
                    throw new ArgumentException("", nameof(mode));
                }
            }
        }
    }
}