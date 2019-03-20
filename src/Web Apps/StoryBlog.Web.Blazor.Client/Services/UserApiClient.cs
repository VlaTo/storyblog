using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class UserApiClient : IUserApiClient
    {
        private readonly HttpClient client;
        private readonly Uri baseUri = new Uri("http://localhost:3100/");
        private readonly ILogger logger;

        public UserApiClient(HttpClient client, ILogger<IUserApiClient> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task LoginAsync()
        {
            var requestUri = new Uri(baseUri, "/account/signin");

            try
            {
                logger.LogDebug($"[{nameof(BlogApiClient)}] Requesting stories from \"{requestUri}\"");

                using (var response = await client.GetAsync(requestUri, CancellationToken.None))
                {
                    response.EnsureSuccessStatusCode();
                    logger.LogDebug($"[{nameof(BlogApiClient)}] Stories fetch status {response.StatusCode}");
                }
            }
            catch (HttpRequestException exception)
            {
                logger.LogError(exception, $"Failed to fetch stories from \"{requestUri}\"");
            }
        }
    }
}