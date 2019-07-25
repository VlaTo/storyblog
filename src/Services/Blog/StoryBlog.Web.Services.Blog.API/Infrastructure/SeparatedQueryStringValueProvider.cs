using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Mvc.Routing;
using StoryBlog.Web.Services.Blog.Interop.Core.Annotations;

namespace StoryBlog.Web.Services.Blog.API.Infrastructure
{
    internal sealed class SeparatedQueryStringValueProvider : BindingSourceValueProvider
    {
        private readonly IQueryCollection query;
        private readonly EnumValuesContext context;
        private PrefixContainer prefixContainer;
        private readonly Collection<MappedValueComparison> comparisons;

        private PrefixContainer PrefixContainer
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
            EnumValuesContext context)
            : base(bindingSource)
        {
            if (null == context)
            {
                throw new ArgumentNullException(nameof(context));
            }

            this.query = query;
            this.context = context;

            comparisons = CreateEnumComparisons();
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
                var decoded = HttpUtility.UrlDecode(value) ?? value;
                var strings = decoded.Split(context.Separator, StringSplitOptions.RemoveEmptyEntries);

                if (false == ValidateEnumValues(strings))
                {
                    throw new InvalidEnumArgumentException();
                }

                result = StringValues.Concat(result, strings);
            }

            return new ValueProviderResult(result, CultureInfo.InvariantCulture);
        }

        private bool ValidateEnumValues(IEnumerable<string> values)
        {
            foreach (var value in values)
            {
                var found = comparisons.Any(comparison => comparison.Equals(value));

                if (found)
                {
                    continue;
                }

                return false;
            }

            return true;
        }

        private Collection<MappedValueComparison> CreateEnumComparisons()
        {
            var fields = context.EnumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            var values = Enum.GetValues(context.EnumType);
            var comparer = StringComparer.InvariantCultureIgnoreCase;
            var result = new Collection<MappedValueComparison>();

            foreach (var value in values)
            {
                if (false == Enum.IsDefined(context.EnumType, value))
                {
                    throw new Exception();
                }

                var name = Enum.GetName(context.EnumType, value);
                var property = fields.FirstOrDefault(field => comparer.Equals(field.Name, name));

                if (null == property)
                {
                    continue;
                }

                var attribute = property.GetCustomAttribute<FlagAttribute>();

                if (null != attribute)
                {
                    result.Add(new MappedValueComparison(attribute.Key, comparer));
                }

                result.Add(new MappedValueComparison(name, comparer));
            }

            return result;
        }

        private class MappedValueComparison
        {
            private readonly string key;
            private readonly IEqualityComparer<string> condition;

            public MappedValueComparison(string key, IEqualityComparer<string> condition)
            {
                this.key = key;
                this.condition = condition;
            }

            public bool Equals(string x) => condition.Equals(key, x);
        }
    }
}