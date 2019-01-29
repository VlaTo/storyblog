namespace StoryBlog.Web.Services.Shared.Captcha
{
    public class CaptchaToken
    {
        public BinaryBlob Value
        {
            get;
        }

        public CaptchaToken(BinaryBlob value)
        {
            Value = value;
        }
    }
}