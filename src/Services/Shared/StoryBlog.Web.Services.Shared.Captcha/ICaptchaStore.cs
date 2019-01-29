using Microsoft.AspNetCore.Http;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICaptchaStore
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        GeneratedCaptcha GetCaptcha(HttpContext context, CaptchaToken token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="captcha"></param>
        void SetCaptcha(HttpContext context, GeneratedCaptcha captcha);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="token"></param>
        void InvalidateCaptcha(HttpContext context, CaptchaToken token);
    }
}