using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
                var attribute = parameter.Attributes.OfType<IEnumValuesQuery>().FirstOrDefault();

                if (null == attribute)
                {
                    continue;
                }

                var context = new EnumValuesContext(
                    parameter.ParameterType,
                    CultureInfo.InvariantCulture.TextInfo.ListSeparator
                );
                
                parameter.Action.Filters.Add(new SeparatedQueryFilter(parameter.BindingInfo.BindingSource, context));
            }
        }
    }
}