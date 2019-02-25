namespace StoryBlog.Web.Services.Shared.Captcha
{
    public sealed class CaptchaValidationResult
    {
        private readonly ValidationResult result;

        /// <summary>
        /// 
        /// </summary>
        public bool IsSuccess => ValidationResult.Success == result;

        /// <summary>
        /// 
        /// </summary>
        public bool IsFailed => ValidationResult.Success != result;

        public CaptchaValidationResult(ValidationResult result)
        {
            this.result = result;
        }
    }
}
