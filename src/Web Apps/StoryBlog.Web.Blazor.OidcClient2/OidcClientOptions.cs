using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StoryBlog.Web.Blazor.OidcClient2.Browser;

namespace StoryBlog.Web.Blazor.OidcClient2
{
    /// <summary>
    /// The authentication flows
    /// </summary>
    public enum AuthenticationFlow
    {
        /// <summary>
        /// authorization code
        /// </summary>
        AuthorizationCode,

        /// <summary>
        /// hybrid
        /// </summary>
        Hybrid
    }

    /// <summary>
    /// The response mode
    /// </summary>
    public enum AuthorizeResponseMode
    {
        /// <summary>
        /// form post
        /// </summary>
        FormPost,

        /// <summary>
        /// redirect
        /// </summary>
        Redirect
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class OidcClientOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public string Authority
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClientSecret
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ProviderInformation ProviderInformation
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Scope
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string RedirectUri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the post logout redirect URI.
        /// </summary>
        /// <value>
        /// The post logout redirect URI.
        /// </value>
        public string PostLogoutRedirectUri
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the browser implementation.
        /// </summary>
        /// <value>
        /// The browser.
        /// </value>
        [JsonIgnore]
        public IBrowser Browser
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the timeout for browser invisible mode.
        /// </summary>
        /// <value>
        /// The browser invisible timeout.
        /// </value>
        public TimeSpan BrowserTimeout
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the clock skew for validating identity tokens.
        /// </summary>
        /// <value>
        /// The clock skew.
        /// </value>
        public TimeSpan ClockSkew { get; set; } = TimeSpan.FromMinutes(5);

        /// <summary>
        /// Gets or sets a value indicating whether the discovery document is re-loaded for every login/prepare login request
        /// </summary>
        /// <value>
        /// <c>true</c> if discovery document needs to be re-loaded; otherwise, <c>false</c>.
        /// </value>
        public bool RefreshDiscoveryDocumentForLogin { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the discovery document gets re-loaded when token validation fails due to signing key problems
        /// </summary>
        /// <value>
        /// <c>true</c> if discovery get re-loaded; otherwise, <c>false</c>.
        /// </value>
        public bool RefreshDiscoveryOnSignatureFailure { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether a response_mode of form_post will be used.
        /// </summary>
        /// <value>
        ///   <c>true</c> for using form_post; otherwise, <c>false</c>.
        /// </value>
        public AuthorizeResponseMode ResponseMode { get; set; } = AuthorizeResponseMode.FormPost;

        /// <summary>
        /// Gets or sets a value indicating whether claims are loaded from the userinfo endpoint
        /// </summary>
        /// <value>
        ///   <c>true</c> for loading claims from userinfo; otherwise, <c>false</c>.
        /// </value>
        public bool LoadProfile { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to filter claims.
        /// </summary>
        /// <value>
        ///   <c>true</c> if claims are filtered; otherwise, <c>false</c>.
        /// </value>
        public bool FilterClaims { get; set; } = true;

        /// <summary>
        /// Gets or sets the flow used for authentication (defaults to hybrid).
        /// </summary>
        /// <value>
        /// The flow.
        /// </value>
        public AuthenticationFlow Flow { get; set; } = AuthenticationFlow.Hybrid;

        /// <summary>
        /// Gets or sets the inner HTTP handler used with RefreshTokenHandler.
        /// </summary>
        /// <value>
        /// The handler.
        /// </value>
        [JsonIgnore]
        public HttpMessageHandler RefreshTokenInnerHttpHandler { get; set; } = new HttpClientHandler();

        /// <summary>
        /// Gets or sets the HTTP handler used for back-channel communication (token and userinfo endpoint).
        /// </summary>
        /// <value>
        /// The backchannel handler.
        /// </value>
        [JsonIgnore]
        public HttpMessageHandler BackchannelHandler { get; set; } = new HttpClientHandler();

        /// <summary>
        /// Gets or sets the backchannel timeout.
        /// </summary>
        /// <value>
        /// The backchannel timeout.
        /// </value>
        public TimeSpan BackchannelTimeout { get; set; } = TimeSpan.FromSeconds(30);

        /// <summary>
        /// Gets or sets the authentication style used by the token client (defaults to posting clientid/secret values).
        /// </summary>
        /// <value>
        /// The token client authentication style.
        /// </value>
        public AuthenticationStyle TokenClientAuthenticationStyle { get; set; } = AuthenticationStyle.PostValues;

        /// <summary>
        /// Gets or sets the policy.
        /// </summary>
        /// <value>
        /// The policy.
        /// </value>
        public Policy Policy { get; set; } = new Policy();

        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        /// <value>
        /// The logger factory.
        /// </value>
        [JsonIgnore]
        public ILoggerFactory LoggerFactory { get; } = new LoggerFactory();

        /// <summary>
        /// Gets or sets the claims types that should be filtered.
        /// </summary>
        /// <value>
        /// The filtered claims.
        /// </value>
        public ICollection<string> FilteredClaims { get; set; } = new HashSet<string>
        {
            JwtClaimTypes.Issuer,
            JwtClaimTypes.Expiration,
            JwtClaimTypes.NotBefore,
            JwtClaimTypes.Audience,
            JwtClaimTypes.Nonce,
            JwtClaimTypes.IssuedAt,
            JwtClaimTypes.AuthenticationTime,
            JwtClaimTypes.AuthorizationCodeHash,
            JwtClaimTypes.AccessTokenHash
        };
    }
}