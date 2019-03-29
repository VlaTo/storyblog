using System.Linq;
using StoryBlog.Web.Blazor.OidcClient2.Infrastructure;

namespace StoryBlog.Web.Blazor.OidcClient2
{
    /// <summary>
    /// Helper class for creating request URLs
    /// </summary>
    public sealed class RequestUrl
    {
        private readonly string _baseUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestUrl"/> class.
        /// </summary>
        /// <param name="baseUrl">The authorize endpoint.</param>
        public RequestUrl(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        /// <summary>
        /// Creates URL based on key/value input pairs.
        /// </summary>
        /// <param name="values">The values (either as a Dictionary of string/string or as a type with properties).</param>
        /// <returns></returns>
        public string Create(object values)
        {
            var dictionary = ValuesHelper.ObjectToDictionary(values);

            if (null == dictionary || false == dictionary.Any())
            {
                return _baseUrl;
            }

            return QueryHelpers.AddQueryString(_baseUrl, dictionary);
        }
    }
}