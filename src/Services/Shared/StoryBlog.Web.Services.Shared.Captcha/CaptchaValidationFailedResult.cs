using Microsoft.AspNetCore.Mvc;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CaptchaValidationFailedResult : BadRequestResult, ICaptchaValidationFailedResult
    {
        /// <inheritdoc />
        public ValidationResult Result
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public CaptchaValidationFailedResult(ValidationResult result)
        {
            Result = result;
        }
    }
}