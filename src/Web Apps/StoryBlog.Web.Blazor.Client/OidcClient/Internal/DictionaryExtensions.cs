using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.OidcClient.Internal
{
    internal static class DictionaryExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="allowEmpty"></param>
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
                if (false == allowEmpty)
                {
                    throw new ArgumentException("Parameter is required", key);
                }

                dictionary.Add(key, String.Empty);
            }
        }
    }
}