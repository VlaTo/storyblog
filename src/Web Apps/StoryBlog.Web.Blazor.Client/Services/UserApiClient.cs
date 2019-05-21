using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using StoryBlog.Web.Blazor.Client.Extensions;
using StoryBlog.Web.Blazor.Client.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class UserApiClient : IUserApiClient
    {
        private readonly IUriHelper uri;
        private readonly HttpClient client;
        private readonly AuthorizationContext authorizationContext;
        private readonly UserApiClientOptions options;
        private readonly DiscoveryCache cache;
        private readonly CryptoHelper crypto;

        public UserApiClient(
            IUriHelper uri,
            HttpClient client,
            AuthorizationContext authorizationContext,
            IOptions<UserApiClientOptions> options)
        {
            this.uri = uri;
            this.client = client;
            this.authorizationContext = authorizationContext;
            this.options = options.Value;
            cache = new DiscoveryCache(this.options.Address, client);
            crypto = new CryptoHelper();
        }

        /// <inheritdoc cref="IUserApiClient.SigninAsync" />
        public async Task SigninAsync()
        {
            var pkce = crypto.CreatePkceData();
            var disco = await cache.GetAsync();
            var url = new RequestUrl(disco.AuthorizeEndpoint);
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

            uri.NavigateTo(auth);
        }

        /// <inheritdoc cref="IUserApiClient.SigninCallbackAsync" />
        public async Task<IPrincipal> SigninCallbackAsync()
        {
            var disco = await cache.GetAsync();
            var token = await RetrieveTokenAsync(disco);

            if (null == token)
            {
                throw new Exception();
            }

            authorizationContext.Token = new AuthorizationToken("Bearer", token);

            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
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

        private async Task<string> RetrieveTokenAsync(DiscoveryResponse disco)
        {
            var path = new Uri(uri.GetAbsoluteUri());
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
                Address = disco.TokenEndpoint,
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