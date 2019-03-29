using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace StoryBlog.Web.Blazor.OidcClient2.Infrastructure
{
    /// <summary>
    /// Helpers to deal with key/value pairs
    /// </summary>
    public static class ValuesHelper
    {
        /// <summary>
        /// Converts an object to a dictionary.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static Dictionary<string, string> ObjectToDictionary(object values)
        {
            if (null == values)
            {
                return null;
            }

            if (values is Dictionary<string, string> dictionary)
            {
                return dictionary;
            }

            dictionary = new Dictionary<string, string>();

            foreach (var prop in values.GetType().GetRuntimeProperties())
            {
                var value = prop.GetValue(values) as string;

                if (String.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                dictionary.Add(prop.Name, value);
            }

            return dictionary;
        }

        /// <summary>
        /// Merges two dictionaries
        /// </summary>
        /// <param name="explicitValues">The explicit values.</param>
        /// <param name="additionalValues">The additional values.</param>
        /// <returns></returns>
        public static Dictionary<string, string> Merge(Dictionary<string, string> explicitValues, Dictionary<string, string> additionalValues = null)
        {
            var merged = explicitValues;

            if (null != additionalValues)
            {
                merged = explicitValues
                    .Concat(additionalValues.Where(add => !explicitValues.ContainsKey(add.Key)))
                    .ToDictionary(final => final.Key, final => final.Value);
            }

            return merged;
        }
    }
}