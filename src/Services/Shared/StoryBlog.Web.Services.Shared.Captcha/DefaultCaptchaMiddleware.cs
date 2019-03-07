using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultCaptchaMiddleware
    {
        private const StringComparison Comparison = StringComparison.InvariantCultureIgnoreCase;
        private static readonly ActionDescriptor emptyActionDescriptor;

        private readonly RequestDelegate next;
        private readonly ICaptchaStore store;
        private readonly ICaptchaImageGenerator generator;
        private readonly CaptchaOptions options;
        private ImageCodecInfo imageCodec;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="store"></param>
        /// <param name="generator"></param>
        /// <param name="options"></param>
        public DefaultCaptchaMiddleware(
            RequestDelegate next,
            ICaptchaStore store,
            ICaptchaImageGenerator generator,
            IOptions<CaptchaOptions> options)
        {
            this.next = next;
            this.store = store;
            this.generator = generator;
            this.options = options.Value;
        }

        static DefaultCaptchaMiddleware()
        {
            emptyActionDescriptor = new ActionDescriptor();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext context)
        {
            var requestPath = context.Request.Path;

            if (false == requestPath.StartsWithSegments(options.RequestPath, Comparison))
            {
                return next.Invoke(context);
            }

            if (false == String.Equals("GET", context.Request.Method, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequestAsync(context);
            }

            var captchaFeature = DefaultCaptcha.GetCaptchaFeature(context);

            if (captchaFeature.HasCaptchaToken)
            {
                var captcha = store.GetCaptcha(context, captchaFeature.CaptchaToken);

                if (null != captcha && DefaultCaptcha.NotExpired(captcha, options.Timeout))
                {
                    return ImageContentAsync(context, captcha);
                }
            }

            return NotFoundAsync(context);
        }

        private async Task ImageContentAsync(HttpContext context, GeneratedCaptcha info)
        {
            var routeData = context.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(context, routeData, emptyActionDescriptor);
            var headers = new RequestHeaders(context.Request.Headers);

            using (var stream = new MemoryStream())
            {
                var codec = GetImageCodec(options.Image.Format);

                if (false == CanClientAccept(headers.Accept, codec.MimeType))
                {
                    return;
                }

                using (var image = await generator.GenerateImageAsync(info.Captcha))
                {
                    var encoderContext = new GeneratedCaptchaImageContext(codec);
                    var args = options.Image.EncoderParameters.Invoke(encoderContext);

                    image.Save(stream, codec, args);
                }

                var contentType = new MediaTypeHeaderValue(codec.MimeType, 1.0d);
                var result = new FileContentResult(stream.ToArray(), contentType);

                await result.ExecuteResultAsync(actionContext);
            }
        }

        private static async Task NotFoundAsync(HttpContext context)
        {
            var routeData = context.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(context, routeData, emptyActionDescriptor);
            var result = new NotFoundResult();

            await result.ExecuteResultAsync(actionContext);
        }

        private ImageCodecInfo GetImageCodec(ImageFormat format)
        {
            if (null == imageCodec)
            {
                var imageEncoders = ImageCodecInfo.GetImageEncoders();
                imageCodec = Array.Find(imageEncoders, codec => codec.FormatID == format.Guid);
            }

            return imageCodec;
        }

        private static async Task BadRequestAsync(HttpContext context)
        {
            var routeData = context.GetRouteData() ?? new RouteData();
            var actionContext = new ActionContext(context, routeData, emptyActionDescriptor);
            var result = new BadRequestResult();

            await result.ExecuteResultAsync(actionContext);
        }

        private static bool CanClientAccept(IEnumerable<MediaTypeHeaderValue> values, string mimeType)
        {
            const string any = "image/*";
            const StringComparison comparison = StringComparison.InvariantCulture;

            foreach (var headerValue in values)
            {
                var mediaType = headerValue.MediaType;

                if (mediaType.Equals(mimeType, comparison))
                {
                    return true;
                }

                if (mediaType.Equals(any, comparison))
                {
                    return true;
                }

                if (mediaType.StartsWith("*", comparison))
                {
                    return true;
                }
            }

            return false;
        }
    }
}