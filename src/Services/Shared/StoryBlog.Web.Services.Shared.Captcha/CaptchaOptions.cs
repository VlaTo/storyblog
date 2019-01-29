using System;
using Microsoft.AspNetCore.Http;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    public enum CaptchaComparisonMode
    {
        /// <summary>
        /// 
        /// </summary>
        CaseSensitive,

        /// <summary>
        /// 
        /// </summary>
        CaseInsensitive
    }

    /// <summary>
    /// 
    /// </summary>
    public class CaptchaOptions
    {
        private FormFieldBuilder formField;
        private string allowedChars;
        private int captchaLength;
        private PathString requestPath;
        private ImageBuilder imageBuilder;
        private CookieBuilder cookie;

        internal static readonly string DefaultFormField = "__RequestValidationCaptchaToken";

        internal static readonly string DefaultCookiePrefix = "AspNetCore.Captcha.";

        /// <summary>
        /// 
        /// </summary>
        public FormFieldBuilder FormField
        {
            get => formField;
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                formField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string AllowedChars
        {
            get => allowedChars;
            set
            {
                if (null == value)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (0 == value.Length)
                {
                    throw new ArgumentException("", nameof(value));
                }

                allowedChars = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CaptchaLength
        {
            get => captchaLength;
            set
            {
                if (0 >= value)
                {
                    throw new ArgumentException("", nameof(value));
                }

                captchaLength = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CaptchaComparisonMode Comparison
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public PathString RequestPath
        {
            get => requestPath;
            set
            {
                if (null == requestPath)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                requestPath = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ImageBuilder Image
        {
            get => imageBuilder;
            set
            {
                if (null== value)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                imageBuilder = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public CookieBuilder Cookie
        {
            get => cookie;
            set
            {
                if (null == cookie)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                cookie = value;
            }
        }

        public CaptchaOptions()
        {
            formField = new FormFieldBuilder();
            requestPath = new PathString("/captcha");
            imageBuilder = new ImageBuilder();
            cookie = new CookieBuilder();
        }
    }
}