using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Services.Identity.API.Services;

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

            services
                .AddScoped<IEmailSender, SimpleEmailSender>()
                .AddOptions<SimpleEmailSenderOptions>()
                .Configure(configurator);

            return services;
        }
    }
}