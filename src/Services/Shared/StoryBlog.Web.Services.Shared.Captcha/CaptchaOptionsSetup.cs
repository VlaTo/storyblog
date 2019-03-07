using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Services.Shared.Captcha.Internal;
using System;
using System.Linq;
using System.Text;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    public sealed class CaptchaOptionsSetup : ConfigureOptions<CaptchaOptions>
    {
        public CaptchaOptionsSetup(IOptions<DataProtectionOptions> dataProtectionOptionsSnapshot)
            : base(options => ConfigureOptions(options, dataProtectionOptionsSnapshot.Value))
        {
        }

        public static void ConfigureOptions(CaptchaOptions options, DataProtectionOptions dataProtectionOptions)
        {
            if (null == options.FormField.Name)
            {
                options.FormField.Name = CaptchaOptions.DefaultFormField;
            }

            if (null == options.Cookie.Name)
            {
                var applicationId = dataProtectionOptions.ApplicationDiscriminator ?? String.Empty;
                options.Cookie.Name = CaptchaOptions.DefaultCookiePrefix + GenerateCookieSuffix(applicationId);
            }

            if (false == options.Cookie.Expiration.HasValue)
            {
                options.Cookie.Expiration = TimeSpan.FromMinutes(15.0d);
            }

            if (TimeSpan.Zero >= options.Timeout)
            {
                options.Timeout = TimeSpan.FromMinutes(3.0d);
            }
        }

        private static string GenerateCookieSuffix(string applicationId)
        {
            using (var sha256 = CryptographyAlgorithms.CreateSHA256())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(applicationId));
                var subHash = hash.Take(8).ToArray();

                return WebEncoders.Base64UrlEncode(subHash);
            }
        }
    }
}