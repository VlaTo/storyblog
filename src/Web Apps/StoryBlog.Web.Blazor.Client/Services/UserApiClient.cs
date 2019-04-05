using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class UserApiClient : IUserApiClient
    {
        private readonly HttpClient client;

        public UserApiClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<string> LoginAsync()
        {
            //var disco = await client.GetDiscoveryDocumentAsync("http://localhost:3100");

            return await Task.FromResult(String.Empty);

            /*var url = new RequestUrl(new Uri("http://localhost:3100"));


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

            return token;*/
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
    }
}