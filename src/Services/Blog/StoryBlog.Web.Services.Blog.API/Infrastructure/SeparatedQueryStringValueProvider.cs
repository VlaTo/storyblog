using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Globalization;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    public class SeparatedQueryStringValueProvider : BindingSourceValueProvider
    {
        private readonly IQueryCollection query;
        private readonly char separator;
        private PrefixContainer prefixContainer;

        protected PrefixContainer PrefixContainer
        {
            get
            {
                if (null == prefixContainer)
                {
                    prefixContainer = new PrefixContainer(query.Keys);
                }

                return prefixContainer;
            }
        }

        public SeparatedQueryStringValueProvider(
            BindingSource bindingSource,
            IQueryCollection query,
            char separator)
            : base(bindingSource)
        {
            if (null == bindingSource)
            {
                throw new ArgumentNullException(nameof(bindingSource));
            }

            if (null == query)
            {
                throw new ArgumentNullException(nameof(query));
            }

            this.query = query;
            this.separator = separator;
        }

        public override bool ContainsPrefix(string prefix) => PrefixContainer.ContainsPrefix(prefix);

        public override ValueProviderResult GetValue(string key)
        {
            if (null == key)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (0 == key.Length)
            {
                // Top level parameters will fall back to an empty prefix when the parameter name does not
                // appear in any value provider. This would result in the parameter binding to a query string
                // parameter with a empty key (e.g. /User?=test) which isn't a scenario we want to support.
                // Return a "None" result in this event.
                return ValueProviderResult.None;
            }

            var values = query[key];

            if (0 == values.Count)
            {
                return ValueProviderResult.None;
            }

            var result = StringValues.Empty;

            foreach (var value in values)
            {
                var strings = value.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                result = StringValues.Concat(result, strings);
            }

            return new ValueProviderResult(result, CultureInfo.InvariantCulture);
        }
    }
}