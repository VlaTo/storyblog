using Microsoft.AspNetCore.Mvc;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICaptchaValidationFailedResult : IActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        ValidationResult Result
        {
            get;
        }
    }
}