using Microsoft.AspNetCore.Http;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICaptcha
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        void Create(HttpContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        CaptchaValidationResult ValidateRequest(HttpContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        string GetImageUrl(HttpContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        FormFieldDescription GetFormFieldDescription();
    }
}