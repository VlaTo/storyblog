namespace StoryBlog.Web.Blazor.OidcClient2.Browser
{
    /// <summary>
    /// Possible browser results.
    /// </summary>
    public enum BrowserResultType
    {
        /// <summary>
        /// success
        /// </summary>
        Success,
        /// <summary>
        /// HTTP error
        /// </summary>
        HttpError,
        /// <summary>
        /// user cancel
        /// </summary>
        UserCancel,
        /// <summary>
        /// timeout
        /// </summary>
        Timeout,
        /// <summary>
        /// unknown error
        /// </summary>
        UnknownError
    }
}