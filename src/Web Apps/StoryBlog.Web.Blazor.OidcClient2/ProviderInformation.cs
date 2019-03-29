using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace StoryBlog.Web.Blazor.OidcClient2
{
    /// <summary>
    /// Information about an OpenID Connect provider
    /// </summary>
    public sealed class ProviderInformation
    {
        /// <summary>
        /// Gets or sets the name of the issuer.
        /// </summary>
        /// <value>
        /// The name of the issuer.
        /// </value>
        public string IssuerName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the key set.
        /// </summary>
        /// <value>
        /// The key set.
        /// </value>
        public JsonWebKeySet KeySet
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the token endpoint.
        /// </summary>
        /// <value>
        /// The token endpoint.
        /// </value>
        public string TokenEndpoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the authorize endpoint.
        /// </summary>
        /// <value>
        /// The authorize endpoint.
        /// </value>
        public string AuthorizeEndpoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the end session endpoint.
        /// </summary>
        /// <value>
        /// The end session endpoint.
        /// </value>
        public string EndSessionEndpoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the user information endpoint.
        /// </summary>
        /// <value>
        /// The user information endpoint.
        /// </value>
        public string UserInfoEndpoint
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the token end point authentication methods.
        /// </summary>
        /// <value>
        /// The token end point authentication methods.
        /// </value>
        public IEnumerable<string> TokenEndPointAuthenticationMethods { get; set; } = new string[] { };


        /// <summary>
        /// Gets a value indicating whether [supports user information].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports user information]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsUserInfo => false == String.IsNullOrWhiteSpace(UserInfoEndpoint);

        /// <summary>
        /// Gets a value indicating whether [supports end session].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [supports end session]; otherwise, <c>false</c>.
        /// </value>
        public bool SupportsEndSession => false == String.IsNullOrWhiteSpace(EndSessionEndpoint);
    }
}