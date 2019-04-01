﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json.Linq;

namespace StoryBlog.Web.Blazor.OidcClient2
{
    /// <summary>
    /// Extensions for JObject
    /// </summary>
    public static class JObjectExtensions
    {
        /// <summary>
        /// Converts a JSON claims object to a list of Claim
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="excludeKeys">Claims that should be excluded.</param>
        /// <returns></returns>
        public static IEnumerable<Claim> ToClaims(this JObject json, params string[] excludeKeys)
        {
            var claims = new List<Claim>();
            var excludeList = excludeKeys.ToList();

            foreach (var x in json)
            {
                if (excludeList.Contains(x.Key)) continue;

                if (x.Value is JArray array)
                {
                    foreach (var item in array)
                    {
                        claims.Add(new Claim(x.Key, item.ToString()));
                    }
                }
                else
                {
                    claims.Add(new Claim(x.Key, x.Value.ToString()));
                }
            }

            return claims;
        }

        /// <summary>
        /// Tries to get a value from a JObject
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static JToken TryGetValue(this JObject json, string name)
        {
            if (json != null && json.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out JToken value))
            {
                return value;
            }

            return null;
        }

        /// <summary>
        /// Tries to get an int from a JObject
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static int? TryGetInt(this JObject json, string name)
        {
            var value = json.TryGetString(name);

            if (value != null)
            {
                if (int.TryParse(value, out int intValue))
                {
                    return intValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Tries to get a string from a JObject
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string TryGetString(this JObject json, string name)
        {
            var value = json.TryGetValue(name);
            return value?.ToString();
        }

        /// <summary>
        /// Tries to get a boolean from a JObject
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool? TryGetBoolean(this JObject json, string name)
        {
            var value = json.TryGetString(name);

            if (bool.TryParse(value, out bool result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Tries to get a string array from a JObject
        /// </summary>
        /// <param name="json">The json.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static IEnumerable<string> TryGetStringArray(this JObject json, string name)
        {
            var values = new List<string>();

            if (json.TryGetValue(name) is JArray array)
            {
                foreach (var item in array)
                {
                    values.Add(item.ToString());
                }
            }

            return values;
        }
    }
}