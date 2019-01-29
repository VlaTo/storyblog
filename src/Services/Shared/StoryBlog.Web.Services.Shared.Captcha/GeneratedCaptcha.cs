using System;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GeneratedCaptcha
    {
        public CaptchaToken Token
        {
            get;
        }

        public char[] Captcha
        {
            get;
        }

        public DateTime CreatedAt
        {
            get;
        }

        public GeneratedCaptcha(CaptchaToken token, char[] captcha, DateTime createdAt)
        {
            Token = token;
            Captcha = captcha;
            CreatedAt = createdAt;
        }
    }
}