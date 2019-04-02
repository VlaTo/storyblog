using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using StoryBlog.Web.Blazor.Client.OidcClient.Internal;

namespace StoryBlog.Web.Blazor.Client.OidcClient.Messages
{
    /// <summary>
    /// Models an OpenID Connect userinfo response
    /// </summary>
    /// <seealso cref="Response" />
    public class UserInfoResponse : Response
    {
        /// <summary>
        /// Gets the claims.
        /// </summary>
        /// <value>
        /// The claims.
        /// </value>
        public IEnumerable<Claim> Claims
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoResponse"/> class.
        /// </summary>
        /// <param name="raw">The raw response data.</param>
        public UserInfoResponse(string raw)
            : base(raw)
        {
            if (false == IsError)
            {
                Claims = Dictionary.ToClaims();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoResponse"/> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public UserInfoResponse(Exception exception)
            : base(exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserInfoResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="reason">The reason.</param>
        public UserInfoResponse(HttpStatusCode statusCode, string reason)
            : base(statusCode, reason)
        {
        }
    }
}