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

        public CommaSeparatedFlagsQueryStringFilter(BindingSource bindingSource, char separator)
        {
            factory = new SeparatedQueryStringValueProviderFactory(bindingSource, separator);
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            context.ValueProviderFactories.Insert(0, factory);
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            /*if (context.Canceled)
            {
                return;
            }

            var attributes = context.Filters.OfType<CommaSeparatedFlagsQueryStringFilter>();

            foreach (var attribute in attributes)
            {
                context.Filters.Remove(attribute);
            }*/
        }
    }
}