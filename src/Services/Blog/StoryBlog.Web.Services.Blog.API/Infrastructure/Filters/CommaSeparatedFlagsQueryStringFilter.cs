using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class CommaSeparatedFlagsQueryStringFilter : IResourceFilter
    {
        private readonly SeparatedQueryStringValueProviderFactory factory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bindingSource"></param>
        /// <param name="separator"></param>
        public CommaSeparatedFlagsQueryStringFilter(BindingSource bindingSource, char separator)
        {
            factory = new SeparatedQueryStringValueProviderFactory(bindingSource, separator);
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