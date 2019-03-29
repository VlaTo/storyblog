﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace StoryBlog.Web.Blazor.OidcClient2.Infrastructure
{
    internal static class QueryHelpers
    {
        /// <summary>
        /// Append the given query key and value to the URI.
        /// </summary>
        /// <param name="uri">The base URI.</param>
        /// <param name="name">The name of the query key.</param>
        /// <param name="value">The query value.</param>
        /// <returns>The combined result.</returns>
        public static string AddQueryString(string uri, string name, string value)
        {
            if (null == uri)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (null == value)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return AddQueryString(uri, new[] {new KeyValuePair<string, string>(name, value)});
        }

        /// <summary>
        /// Append the given query keys and values to the uri.
        /// </summary>
        /// <param name="uri">The base uri.</param>
        /// <param name="queryString">A collection of name value query pairs to append.</param>
        /// <returns>The combined result.</returns>
        public static string AddQueryString(string uri, IDictionary<string, string> queryString)
        {
            if (null == uri)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (null == queryString)
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            return AddQueryString(uri, (IEnumerable<KeyValuePair<string, string>>)queryString);
        }

        private static string AddQueryString(string uri, IEnumerable<KeyValuePair<string, string>> queryString)
        {
            if (null == uri)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (null == queryString)
            {
                throw new ArgumentNullException(nameof(queryString));
            }

            var anchorIndex = uri.IndexOf('#');
            var uriToBeAppended = uri;
            var anchorText = String.Empty;

            // If there is an anchor, then the query string must be inserted before its first occurance.
            if (-1 < anchorIndex)
            {
                anchorText = uri.Substring(anchorIndex);
                uriToBeAppended = uri.Substring(0, anchorIndex);
            }

            var queryIndex = uriToBeAppended.IndexOf('?');
            var hasQuery = -1 != queryIndex;

            var sb = new StringBuilder();
            sb.Append(uriToBeAppended);

            foreach (var parameter in queryString)
            {
                if (null == parameter.Value)
                {
                    continue;
                }

                sb.Append(hasQuery ? '&' : '?');
                sb.Append(UrlEncoder.Default.Encode(parameter.Key));
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(parameter.Value));

                hasQuery = true;
            }

            sb.Append(anchorText);

            return sb.ToString();
        }
    }
}