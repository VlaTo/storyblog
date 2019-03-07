using System;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CaptchaValidationResult
    {
        //private readonly ValidationResult result;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess => ValidationResult.Success == Result;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFailed => ValidationResult.Success != Result;

        /// <summary>
        /// 
        /// </summary>
        public ValidationResult Result
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public CaptchaValidationResult(ValidationResult result)
        {
            Result = result;
        }
    }
}
