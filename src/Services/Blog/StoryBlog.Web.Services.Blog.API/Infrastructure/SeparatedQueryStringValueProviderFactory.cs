using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    public class SeparatedQueryStringValueProviderFactory : IValueProviderFactory
    {
        private readonly BindingSource bindingSource;
        private readonly char separator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingSource"></param>
        /// <param name="parameterType"></param>
        /// <param name="key"></param>
        /// <param name="separator"></param>
        public SeparatedQueryStringValueProviderFactory(BindingSource bindingSource, char separator)
        {
            this.bindingSource = bindingSource;
            this.separator = separator;
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
                    new SeparatedQueryStringValueProvider(bindingSource, query, separator)
                );
            }

            return Task.CompletedTask;
        }
    }
}