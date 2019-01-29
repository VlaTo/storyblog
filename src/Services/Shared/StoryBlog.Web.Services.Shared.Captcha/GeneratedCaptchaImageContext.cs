using System.Drawing.Imaging;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    public sealed class GeneratedCaptchaImageContext
    {
        public ImageCodecInfo ImageCodec
        {
            get;
        }

        public GeneratedCaptchaImageContext(ImageCodecInfo imageCodec)
        {
            ImageCodec = imageCodec;
        }
    }
}