using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    internal sealed class SeparatedQueryValueProviderFactory : IValueProviderFactory
    {
        private readonly BindingSource bindingSource;
        private readonly EnumValuesContext enumValuesContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingSource"></param>
        /// <param name="enumValuesContext"></param>
        /// <param name="separator"></param>
        /// <param name="parameterType"></param>
        /// <param name="key"></param>
        public SeparatedQueryValueProviderFactory(BindingSource bindingSource, EnumValuesContext enumValuesContext)
        {
            this.bindingSource = bindingSource;
            this.enumValuesContext = enumValuesContext;
        }

        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var query = context.ActionContext.HttpContext.Request.Query;

            if (null != query && 0 < query.Count)
            {
                context.ValueProviders.Add(
                    new SeparatedQueryStringValueProvider(bindingSource, query, enumValuesContext)
                );
            }

            return Task.CompletedTask;
        }
    }
}