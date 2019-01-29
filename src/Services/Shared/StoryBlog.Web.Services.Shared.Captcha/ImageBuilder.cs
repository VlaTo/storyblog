using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    public delegate EncoderParameters ImageEncoderParametersDelegate(GeneratedCaptchaImageContext context);

    public class ImageBuilder
    {
        internal static readonly Size DefaultImageSize = new Size(240, 80);

        private ImageFormat format;
        private Size size;
        private ImageEncoderParametersDelegate encoderParameters;

        public Size Size
        {
            get => size;
            set
            {
                if (value.IsEmpty)
                {
                    throw new ArgumentException("", nameof(value));
                }

                size = value;
            }
        }

        public ImageFormat Format
        {
            get => format;
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                format = value;
            }
        }

        public ImageEncoderParametersDelegate EncoderParameters
        {
            get => encoderParameters;
            set
            {
                if (null == encoderParameters)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                encoderParameters = value;
            }
        }

        public ImageBuilder()
        {
            format = ImageFormat.Png;
            size = new Size(240, 80);
            encoderParameters = context => new EncoderParameters(1)
            {
                Param =
                {
                    [0] = new EncoderParameter(Encoder.Quality, 50L)
                }
            };
        }
    }
}