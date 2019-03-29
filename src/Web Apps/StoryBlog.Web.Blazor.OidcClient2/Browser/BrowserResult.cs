﻿using StoryBlog.Web.Blazor.OidcClient2.Results;

namespace StoryBlog.Web.Blazor.OidcClient2.Browser
{
    /// <summary>
    /// The result from a browser login.
    /// </summary>
    /// <seealso cref="Result" />
    public class BrowserResult : Result
    {
        /// <summary>
        /// Gets or sets the type of the result.
        /// </summary>
        /// <value>
        /// The type of the result.
        /// </value>
        public BrowserResultType ResultType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        public string Response
        {
            get;
            set;
        }
    }
}