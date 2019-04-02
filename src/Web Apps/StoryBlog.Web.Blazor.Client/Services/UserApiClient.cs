using System.Net.Http;
using System.Threading.Tasks;
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

                logger.LogDebug($"Access token: {response.AccessToken}");
                logger.LogDebug($"Identity token: {response.IdentityToken}");

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