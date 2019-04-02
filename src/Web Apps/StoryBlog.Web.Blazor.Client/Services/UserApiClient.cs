﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Blazor.Client.OidcClient;
using StoryBlog.Web.Blazor.Client.OidcClient.Messages;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class UserApiClient : IUserApiClient
    {
        private readonly HttpClient client;
        private readonly ILogger logger;

        public UserApiClient(HttpClient client, ILogger<IUserApiClient> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task<string> LoginAsync()
        {
            string token = null;

            try
            {
                var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = "http://localhost:3100/connect/token",
                    ClientCredentialStyle = ClientCredentialStyle.PostBody,
                    ClientId = "client.application",
                    ClientSecret = "secret",
                    Scope = "api.blog"
                });

                if (response.IsError)
                {
                    throw new Exception();
                }

                token = response.AccessToken;
            }
            catch (HttpRequestException exception)
            {
                logger.LogError(exception, "Failed");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed");
            }

            return token;
        }

        public async Task<IEnumerable<Claim>> GetUserInfoAsync(string token)
        {
            try
            {
                var response = await client.GetUserInfoAsync(new UserInfoRequest
                {
                    Address = "http://localhost:3100/connect/userinfo",
                    Token = token
                });

                if (response.IsError)
                {
                    throw new Exception();
                }

                return response.Claims;
            }
            catch (HttpRequestException exception)
            {
                logger.LogError(exception, "Failed");
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Failed");
            }

            return Enumerable.Empty<Claim>();
        }
    }
}