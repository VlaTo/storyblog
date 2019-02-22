using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace StoryBlog.Web.Services.Shared.Common
{
    /// <summary>
    /// 
    /// </summary>
    public struct QueryString : IEquatable<QueryString>
    {
        private static readonly Func<string, string> encode;

        /// <summary>
        /// 
        /// </summary>
        public static readonly QueryString Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsEmpty => String.IsNullOrEmpty(Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public QueryString(string value)
        {
            if (false == String.IsNullOrEmpty(value) && value[0] != '?')
            {
                throw new ArgumentException("", nameof(value));
            }

            Value = value;
        }

        static QueryString()
        {
            Empty = new QueryString(String.Empty);
            encode = WebUtility.UrlEncode;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public QueryString Add(string name, string val)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (IsEmpty || Value.Equals("?", StringComparison.Ordinal))
            {
                return Create(name, val);
            }

            var builder = new StringBuilder(Value);

            AppendKeyValuePair(builder, name, val, first: false);

            return new QueryString(builder.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public QueryString Concat(QueryString other)
        {
            if (IsEmpty || Value.Equals("?", StringComparison.Ordinal))
            {
                return other;
            }

            if (other.IsEmpty || other.Value.Equals("?", StringComparison.Ordinal))
            {
                return this;
            }

            // ?name1=value1 Add ?name2=value2 returns ?name1=value1&name2=value2
            return new QueryString(Value + "&" + other.Value.Substring(1));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(QueryString other)
        {
            if (IsEmpty && other.IsEmpty)
            {
                return true;
            }

            return String.Equals(Value, other.Value, StringComparison.Ordinal);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return IsEmpty;
            }

            return obj is QueryString other && Equals(other);
        }

        /// <inheritdoc cref="ValueType.GetHashCode"/>
        public override int GetHashCode() => IsEmpty ? 0 : Value.GetHashCode();

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString() => ToUriComponent();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToUriComponent() => IsEmpty ? String.Empty : Value.Replace("#", "%23");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uriComponent"></param>
        /// <returns></returns>
        public static QueryString FromUriComponent(string uriComponent)
        {
            if (String.IsNullOrEmpty(uriComponent))
            {
                return Empty;
            }

            return new QueryString(uriComponent);
        }

        /// <summary>
        /// Returns an QueryString given the query as from a Uri object. Relative Uri objects are not supported.
        /// </summary>
        /// <param name="uri">The Uri object</param>
        /// <returns>The resulting QueryString</returns>
        public static QueryString FromUriComponent(Uri uri)
        {
            if (null == uri)
            {
                throw new ArgumentNullException(nameof(uri));
            }

            var queryValue = uri.GetComponents(UriComponents.Query, UriFormat.UriEscaped);

            if (false == String.IsNullOrEmpty(queryValue))
            {
                queryValue = "?" + queryValue;
            }

            return new QueryString(queryValue);
        }

        /// <summary>
        /// Create a query string with a single given parameter name and value.
        /// </summary>
        /// <param name="name">The un-encoded parameter name</param>
        /// <param name="value">The un-encoded parameter value</param>
        /// <returns>The resulting QueryString</returns>
        public static QueryString Create(string name, string value)
        {
            if (null == name)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (false == String.IsNullOrEmpty(value))
            {
                value = encode.Invoke(value);
            }

            return new QueryString($"?{encode.Invoke(name)}={value}");
        }

        /// <summary>
        /// Creates a query string composed from the given name value pairs.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>The resulting QueryString</returns>
        public static QueryString Create(IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var builder = new StringBuilder();
            var first = true;

            foreach (var kvp in parameters)
            {
                AppendKeyValuePair(builder, kvp.Key, kvp.Value, first);
                first = false;
            }

            return new QueryString(builder.ToString());
        }

        /// <summary>
        /// Creates a query string composed from the given name value pairs.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns>The resulting QueryString</returns>
        public static QueryString Create(IEnumerable<KeyValuePair<string, StringValues>> parameters)
        {
            var builder = new StringBuilder();
            var first = true;

            foreach (var pair in parameters)
            {
                // If nothing in this pair.Values, append null value and continue
                if (StringValues.IsNullOrEmpty(pair.Value))
                {
                    AppendKeyValuePair(builder, pair.Key, null, first);
                    first = false;
                    continue;
                }

                // Otherwise, loop through values in pair.Value
                foreach (var value in pair.Value)
                {
                    AppendKeyValuePair(builder, pair.Key, value, first);
                    first = false;
                }
            }

            return new QueryString(builder.ToString());
        }

        public static bool operator ==(QueryString left, QueryString right) => left.Equals(right);

        public static bool operator !=(QueryString left, QueryString right) => false == left.Equals(right);

        public static QueryString operator +(QueryString left, QueryString right) => left.Concat(right);

        public static explicit operator string(QueryString queryString) => queryString.ToUriComponent();

        private static void AppendKeyValuePair(StringBuilder builder, string key, string value, bool first)
        {
            builder.Append(first ? "?" : "&");
            builder.Append(encode.Invoke(key));
            builder.Append("=");

            if (false == String.IsNullOrEmpty(value))
            {
                builder.Append(encode.Invoke(value));
            }
        }
    }
}