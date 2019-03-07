using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace StoryBlog.Web.Services.Shared.Captcha.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCaptcha(this IServiceCollection services)
        {
            if (null == services)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddDataProtection();

            services.TryAddTransient<ValidateCaptchaTokenAuthorizationFilter>();
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<CaptchaOptions>, CaptchaOptionsSetup>()
            );
            services.TryAddSingleton<ICaptchaImageGenerator, DefaultCaptchaImageGenerator>();
            services.TryAddSingleton<ICaptchaStore, DefaultCaptchaStore>();
            services.TryAddSingleton<ICaptcha, DefaultCaptcha>();

            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="setup"></param>
        /// <returns></returns>
        public static IServiceCollection AddCaptcha(this IServiceCollection services, Action<CaptchaOptions> setup)
        {
            if (null == services)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (null == setup)
            {
                throw new ArgumentNullException(nameof(setup));
            }

            services.AddCaptcha();
            services.Configure(setup);

            return services;
        }
    }
}