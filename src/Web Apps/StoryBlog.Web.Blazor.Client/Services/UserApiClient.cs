using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Blazor.OidcClient2;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class UserApiClient : IUserApiClient
    {
        private readonly HttpClient client;
        //private readonly Uri baseUri = new Uri("http://localhost:3100/");
        private readonly ILogger logger;

        public UserApiClient(HttpClient client, ILogger<IUserApiClient> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task<string> LoginAsync()
        {
            try
            {
                var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = "http://localhost:3100",
                    ClientCredentialStyle = ClientCredentialStyle.PostBody,
                    ClientId = "client.application",
                    ClientSecret = "secret"
                });

                return response.AccessToken;
            }
            catch (HttpRequestException exception)
            {
                logger.LogError(exception, "Failed");
            }

            return null;
        }
    }
}