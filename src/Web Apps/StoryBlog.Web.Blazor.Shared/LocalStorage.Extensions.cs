/*
 *
 */

using Microsoft.Extensions.DependencyInjection;

namespace StoryBlog.Web.Blazor.Shared
{
    /// <summary>
    /// 
    /// </summary>
    public static class LocalStorageExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLocalStorage(this IServiceCollection services)
        {
            //services.Add(ServiceDescriptor.Singleton<ILocalStorage, LocalStorage>());

            return services;
        }
    }
}