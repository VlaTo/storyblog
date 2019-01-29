namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICaptchaFeature
    {
        /// <summary>
        /// 
        /// </summary>
        bool HasCaptchaToken
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        CaptchaToken CaptchaToken
        {
            get;
            set;
        }
    }
}