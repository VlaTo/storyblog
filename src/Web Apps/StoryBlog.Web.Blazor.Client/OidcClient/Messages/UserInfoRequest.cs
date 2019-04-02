namespace StoryBlog.Web.Blazor.Client.OidcClient.Messages
{
    /// <summary>
    /// Request for OIDC userinfo
    /// </summary>
    public sealed class UserInfoRequest
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token
        {
            get;
            set;
        }
    }
}