namespace StoryBlog.Web.Blazor.OidcClient2
{
    /// <summary>
    /// Enum for specifying the authentication style of a client
    /// </summary>
    public enum AuthenticationStyle
    {
        /// <summary>
        /// HTTP basic authentication
        /// </summary>
        BasicAuthentication,

        /// <summary>
        /// post values in body
        /// </summary>
        PostValues,

        /// <summary>
        /// custom
        /// </summary>
        Custom
    };
}