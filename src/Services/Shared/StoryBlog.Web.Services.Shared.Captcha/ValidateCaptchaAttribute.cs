using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using StoryBlog.Web.Services.Identity.API.Infrastructure;

namespace StoryBlog.Web.Services.Shared.Captcha
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateCaptchaAttribute : Attribute, IFilterFactory, IOrderedFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsReusable => true;

        /// <summary>
        /// 
        /// </summary>
        public int Order { get; set; } = 1100;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider) =>
            serviceProvider.GetRequiredService<ValidateCaptchaTokenAuthorizationFilter>();
    }
}