using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Attributes;
using StoryBlog.Web.Services.Blog.API.Infrastructure.Filters;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class CommaSeparatedFlagsQueryStringConvention : IActionModelConvention
    {
        /// <inheritdoc cref="IActionModelConvention.Apply" />
        public void Apply(ActionModel action)
        {
            if (null == action)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var parameter in action.Parameters)
            {
                var attribute = parameter.Attributes.OfType<FromCommaSeparatedQueryAttribute>().FirstOrDefault();

                if (null == attribute)
                {
                    continue;
                }

                parameter.Action.Filters.Add(new CommaSeparatedFlagsQueryStringFilter(
                    parameter.BindingInfo.BindingSource,
                    attribute.Separator
                ));
            }
        }
    }
}