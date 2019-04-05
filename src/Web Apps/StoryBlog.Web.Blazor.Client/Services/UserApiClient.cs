﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Components.Services;
using Microsoft.AspNetCore.WebUtilities;
using StoryBlog.Web.Blazor.Client.Helpers;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class UserApiClient : IUserApiClient
    {
        private readonly IUriHelper uri;
        private readonly HttpClient client;
        private readonly UserApiClientOptions options;
        private readonly DiscoveryCache cache;
        private readonly CryptoHelper crypto;

        public UserApiClient(IUriHelper uri, HttpClient client, UserApiClientOptions options)
        {
            this.uri = uri;
            this.client = client;
            this.options = options;
            cache = new DiscoveryCache(options.Address.Authority, client);
            crypto = new CryptoHelper();
        }

        /// <inheritdoc cref="IUserApiClient.LoginAsync" />
        public async Task LoginAsync()
        {
            var disco = await cache.GetAsync();
            var scopes = String.Join(" ",
                OidcConstants.StandardScopes.OpenId,
                OidcConstants.StandardScopes.Profile,
                "api.blog"
            );
            var redirect = new UriBuilder(client.BaseAddress)
            {
                Path = "callback"
            };
            var pkce = crypto.CreatePkceData();
            var auth = new RequestUrl(disco.AuthorizeEndpoint).CreateAuthorizeUrl(
                options.ClientId,
                OidcConstants.ResponseTypes.Code,
                scopes,
                redirect.ToString(),
                state: pkce.CodeVerifier,
                responseMode: OidcConstants.ResponseModes.Query,
                codeChallenge: pkce.CodeChallenge,
                codeChallengeMethod: OidcConstants.CodeChallengeMethods.Sha256
            );

            uri.NavigateTo(auth);
        }

        /// <inheritdoc cref="IUserApiClient.SigninCallbackAsync" />
        public async Task<IPrincipal> SigninCallbackAsync()
        {
            var disco = await cache.GetAsync();
            var token = await RetrieveTokenAsync(disco);
            var response = await client.GetUserInfoAsync(new UserInfoRequest
            {
                Address = disco.UserInfoEndpoint,
                Token = token
            });

            if (response.IsError)
            {
                throw response.Exception;
            }

            var identity = new ClaimsIdentity(response.Claims);

            return new ClaimsPrincipal(identity);
        }

        public async Task<IEnumerable<Claim>> GetUserInfoAsync(string token)
        {
            try
            {
                /*var response = await client.GetUserInfoAsync(new UserInfoRequest
                {
                    Address = "http://localhost:3100/connect/userinfo",
                    Token = token
                });

                if (response.IsError)
                {
                    throw new Exception();
                }

                return response.Claims;*/
                return await Task.FromResult(Enumerable.Empty<Claim>());
            }
            catch (HttpRequestException exception)
            {
                return await Task.FromResult(Enumerable.Empty<Claim>());
            }
            catch (Exception exception)
            {
                return await Task.FromResult(Enumerable.Empty<Claim>());
            }
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
    }
}