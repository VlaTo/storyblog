using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Blazor.Client.OidcClient.Jwt
{
    /// <summary>
    /// Contains a collection of <see cref="JsonWebKey"/> that can be populated from a json string.
    /// </summary>
    public class JsonWebKeySet
    {
        private readonly List<JsonWebKey> keys = new List<JsonWebKey>();

        /// <summary>
        /// Gets the <see cref="IList{JsonWebKey}"/>.
        /// </summary>       
        public IList<JsonWebKey> Keys => keys;

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKeySet"/>.
        /// </summary>
        public JsonWebKeySet()
        {
        }

        /// <summary>
        /// Initializes an new instance of <see cref="JsonWebKeySet"/> from a json string.
        /// </summary>
        /// <param name="json">a json string containing values.</param>
        /// <exception cref="ArgumentNullException">if 'json' is null or whitespace.</exception>
        public JsonWebKeySet(string json)
        {
            if (String.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            var jwebKeys = Microsoft.JSInterop.Json.Deserialize<JsonWebKeySet>(json);
            //var jwebKeys = JsonConvert.DeserializeObject<JsonWebKeySet>(json);

            keys = jwebKeys.keys;
        }
    }
}