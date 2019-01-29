using System;
using System.Collections.Generic;
using System.Text;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    public sealed class CaptchaValidationResult
    {
        private ValidationResult result;

        public bool IsSuccess => ValidationResult.Success == result;

        public bool IsFailed => ValidationResult.Success != result;


    }
}
