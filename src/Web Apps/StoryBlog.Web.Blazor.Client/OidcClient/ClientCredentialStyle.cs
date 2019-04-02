namespace StoryBlog.Web.Blazor.Client.OidcClient
{
    /// <summary>
    /// Specifies how the client will transmit client ID and secret
    /// </summary>
    public enum ClientCredentialStyle
    {
        /// <summary>
        /// HTTP basic authentication
        /// </summary>
        AuthorizationHeader,

        /// <summary>
        /// Post values in body
        /// </summary>
        PostBody
    }
}