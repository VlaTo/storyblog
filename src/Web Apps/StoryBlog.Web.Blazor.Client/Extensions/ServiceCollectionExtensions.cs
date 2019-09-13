using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Client.Core;
using System;

namespace StoryBlog.Web.Client.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBootstrapModalService(this IServiceCollection services)
        {
            if (null == services)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<IModalService, BootstrapModalService>();

            return services;
        }
    }
}