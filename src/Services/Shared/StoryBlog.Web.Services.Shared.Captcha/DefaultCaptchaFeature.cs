namespace StoryBlog.Web.Services.Shared.Captcha
{
    public class DefaultCaptchaFeature : ICaptchaFeature
    {
        public bool HasCaptchaToken
        {
            get;
            set;
        }

        public CaptchaToken CaptchaToken
        {
            get;
            set;
        }
    }
}