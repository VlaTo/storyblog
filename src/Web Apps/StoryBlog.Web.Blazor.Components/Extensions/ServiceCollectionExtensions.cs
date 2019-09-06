using System;
using Microsoft.Extensions.DependencyInjection;

namespace StoryBlog.Web.Blazor.Components.Extensions
{
    public static class ServiceCollectionExtensions
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