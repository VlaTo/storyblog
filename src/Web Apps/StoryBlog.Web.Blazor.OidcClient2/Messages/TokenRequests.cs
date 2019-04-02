namespace StoryBlog.Web.Blazor.OidcClient2.Messages
{
    /// <summary>
    /// Request for token
    /// </summary>
    /// <seealso cref="Request" />
    public class TokenRequest : Request
    {
        /// <summary>
        /// Gets or sets the type of the grant.
        /// </summary>
        /// <value>
        /// The type of the grant.
        /// </value>
        public string GrantType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Request for token using client_credentials
    /// </summary>
    /// <seealso cref="TokenRequest" />
    public class ClientCredentialsTokenRequest : TokenRequest
    {
        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public string Scope
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Request for token using authorization_code
    /// </summary>
    /// <seealso cref="TokenRequest" />
    public class AuthorizationCodeTokenRequest : TokenRequest
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code
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
    }
}