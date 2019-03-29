namespace StoryBlog.Web.Blazor.OidcClient2
{
    /// <summary>
    /// Represents the state the needs to be hold between starting the authorize request and the response
    /// </summary>
    public class AuthorizeState
    {
        /// <summary>
        /// Gets or sets the start URL.
        /// </summary>
        /// <value>
        /// The start URL.
        /// </value>
        public string StartUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the nonce.
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        public string Nonce
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the code verifier.
        /// </summary>
        /// <value>
        /// The code verifier.
        /// </value>
        public string CodeVerifier
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the redirect URI.
        /// </summary>
        /// <value>
        /// The redirect URI.
        /// </value>
        public string RedirectUri
        {
            get;
            set;
        }
    }
}