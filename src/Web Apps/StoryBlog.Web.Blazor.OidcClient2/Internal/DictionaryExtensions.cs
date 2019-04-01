using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.OidcClient2.Internal
{
    internal static class DictionaryExtensions
    {
        public static void AddIfPresent(
            this IDictionary<string, string> dictionary,
            string key,
            string value)
        {
            if (false == String.IsNullOrWhiteSpace(value))
            {
                dictionary.Add(key, value);
            }
        }

        public static void AddOptional(
            this IDictionary<string, string> dictionary,
            string key,
            string value)
        {
            if (false == String.IsNullOrWhiteSpace(value))
            {
                if (dictionary.ContainsKey(key))
                {
                    throw new InvalidOperationException($"Duplicate parameter: {key}");
                }

                dictionary.Add(key, value);
            }
        }

        public static void AddRequired(
            this IDictionary<string, string> dictionary,
            string key,
            string value,
            bool allowEmpty = false)
        {
            if (false == String.IsNullOrWhiteSpace(value))
            {
                if (dictionary.ContainsKey(key))
                {
                    throw new InvalidOperationException($"Duplicate parameter: {key}");
                }

                dictionary.Add(key, value);
            }
            else
            {
                if (allowEmpty)
                {
                    dictionary.Add(key, "");
                }
                else
                {
                    throw new ArgumentException("Parameter is required", key);
                }
            }
        }
    }
}