using System;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Blazor.Client.Components;
using StoryBlog.Web.Blazor.Client.Core;

namespace StoryBlog.Web.Blazor.Client.Extensions
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