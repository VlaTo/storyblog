using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;

namespace StoryBlog.Web.Client.Services
{
    internal sealed class IdentityApiClient : IIdentityApiClient
    {
        private readonly NavigationManager navigationManager;
        private readonly HttpClient client;
        private readonly IdentityApiOptions options;
        private readonly DiscoveryCache disco;
        private readonly CryptoHelper crypto;

        public IdentityApiClient(
            NavigationManager navigationManager,
            HttpClient client,
            IOptions<IdentityApiOptions> options)
        {
            this.navigationManager = navigationManager;
            this.client = client;
            this.options = options.Value;
            var authority = this.options.Host.ToString();
            disco = new DiscoveryCache(authority, () => this.client);
            crypto = new CryptoHelper();
        }

        /// <inheritdoc cref="IIdentityApiClient.SigninAsync" />
        public async Task SigninAsync()
        {
            var pkce = crypto.CreatePkceData();
            var discovery = await disco.GetAsync();
            var url = new RequestUrl(discovery.AuthorizeEndpoint);
            var auth = url.CreateAuthorizeUrl(
                options.ClientId,
                OidcConstants.ResponseTypes.Code,
                GetScopes(options.Scopes),
                options.RedirectUri,
                state: pkce.CodeVerifier,
                responseMode: OidcConstants.ResponseModes.Query,
                codeChallenge: pkce.CodeChallenge,
                codeChallengeMethod: OidcConstants.CodeChallengeMethods.Sha256,
                uiLocales: CultureInfo.CurrentUICulture.EnglishName
            );

            navigationManager.NavigateTo(auth);
        }

        /// <inheritdoc cref="IIdentityApiClient.SigninCallbackAsync" />
        public async Task<IPrincipal> SigninCallbackAsync()
        {
            var discovery = await disco.GetAsync();
            var token = await RetrieveTokenAsync(discovery);

            if (null == token)
            {
                throw new Exception();
            }

            Console.WriteLine($"Setting bearer token: {token}");
            client.SetBearerToken(token);

            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = discovery.UserInfoEndpoint,
                Token = token
            });

            if (response.IsError)
            {
                throw response.Exception;
            }

            if (null != response.Claims)
            {
                return Principal.Create("local", response.Claims.ToArray());
            }

            return Principal.Anonymous;
        }

        private async Task<string> RetrieveTokenAsync(DiscoveryDocumentResponse discovery)
        {
            var path = new Uri(navigationManager.Uri);
            var query = QueryHelpers.ParseQuery(path.Query);

            if (false == query.TryGetValue("code", out var code))
            {
                throw new Exception();
            }

            if (false == query.TryGetValue("state", out var state))
            {
                throw new Exception();
            }

            var redirect = new UriBuilder(client.BaseAddress)
            {
                Path = "callback"
            };

            var response = await client.RequestAuthorizationCodeTokenAsync(new AuthorizationCodeTokenRequest
            {
                Address = discovery.TokenEndpoint,
                ClientId = options.ClientId,
                Code = code,
                CodeVerifier = state,
                RedirectUri = redirect.ToString(),
                GrantType = OidcConstants.GrantTypes.AuthorizationCode
            });

            if (response.IsError)
            {
                throw response.Exception;
            }

            return response.AccessToken;
        }

        private static string GetScopes(IEnumerable<string> scopes) => String.Join(" ", scopes);
    }
}