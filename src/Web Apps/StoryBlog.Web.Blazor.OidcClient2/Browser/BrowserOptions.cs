using System;

namespace StoryBlog.Web.Blazor.OidcClient2.Browser
{
    /// <summary>
    /// Options for the browser used for login.
    /// </summary>
    public class BrowserOptions
    {
        /// <summary>
        /// Gets the start URL.
        /// </summary>
        /// <value>
        /// The start URL.
        /// </value>
        public string StartUrl
        {
            get;
        }

        /// <summary>
        /// Gets the end URL.
        /// </summary>
        /// <value>
        /// The end URL.
        /// </value>
        public string EndUrl
        {
            get;
        }

        /// <summary>
        /// Gets or sets the OpenID Connect response mode.
        /// </summary>
        /// <value>
        /// The response mode.
        /// </value>
        public AuthorizeResponseMode ResponseMode { get; set; } = AuthorizeResponseMode.FormPost;

        /// <summary>
        /// Gets or sets the browser display mode.
        /// </summary>
        /// <value>
        /// The display mode.
        /// </value>
        public DisplayMode DisplayMode { get; set; } = DisplayMode.Visible;

        /// <summary>
        /// Gets or sets the browser timeout.
        /// </summary>
        /// <value>
        /// The timeout.
        /// </value>
        public TimeSpan Timeout { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserOptions"/> class.
        /// </summary>
        /// <param name="startUrl">The start URL.</param>
        /// <param name="endUrl">The end URL.</param>
        public BrowserOptions(string startUrl, string endUrl)
        {
            StartUrl = startUrl;
            EndUrl = endUrl;
        }
    }
}