using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure.Filters
{
    /// <summary>
    /// 
    /// </summary>
    internal class SeparatedQueryFilter : IResourceFilter
    {
        private readonly SeparatedQueryValueProviderFactory factory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingSource"></param>
        /// <param name="separator"></param>
        public SeparatedQueryFilter(BindingSource bindingSource, EnumValuesContext context)
        {
            factory = new SeparatedQueryValueProviderFactory(bindingSource, context);
        }

        /// <inheritdoc cref="IResourceFilter.OnResourceExecuting" />
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.ValueProviderFactories.Insert(0, factory);
        }

        /// <inheritdoc cref="IResourceFilter.OnResourceExecuted" />
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}