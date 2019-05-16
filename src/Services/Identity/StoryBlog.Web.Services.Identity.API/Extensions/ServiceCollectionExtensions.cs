using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StoryBlog.Web.Services.Identity.Application.Services;
using System;

namespace StoryBlog.Web.Services.Identity.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="SimpleEmailSender" /> as a <see cref="IEmailSender" /> implementation.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configurator"></param>
        /// <returns></returns>
        public static IServiceCollection AddSimpleEmailSender(this IServiceCollection services, Action<SimpleEmailSenderOptions> configurator)
        {
            if (null == services)
            {
                throw new ArgumentNullException(nameof(services));
            }

            //services.AddOptions<SimpleEmailSenderOptions>().Configure(configurator);
            services.Configure(configurator);
            services.TryAddSingleton<IEmailTemplateGenerator, SimpleEmailTemplateGenerator>();
            services.TryAddSingleton<ISimpleEmailSender, SimpleEmailSender>();

            return services;
        }
    }
}