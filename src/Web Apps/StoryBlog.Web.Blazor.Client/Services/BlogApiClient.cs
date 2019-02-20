using StoryBlog.Web.Services.Blog.Common;
using StoryBlog.Web.Services.Blog.Common.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class BlogApiClient : IBlogApiClient
    {
        private readonly HttpClient client;
        private readonly Uri baseUri = new Uri("http://localhost:3000/api/v1/");
        private readonly ILogger logger;

        public BlogApiClient(HttpClient client, ILogger<IBlogApiClient> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task<ListResult<StoryModel>> GetStoriesAsync()
        {
            var requestUri = new Uri(baseUri, "stories");

            logger.LogDebug($"Call to API: {requestUri}");

            try
            {
                //var result = await client.GetJsonAsync<ListResult<StoryModel>>(path);

                using (var stream = await client.GetStreamAsync(requestUri))
                {
                    var json = await stream.GetResponseStringAsync(CancellationToken.None);
                    return Json.Deserialize<ListResult<StoryModel>>(json);
                }
            }
            catch (HttpRequestException exception)
            {
                Debug.WriteLine(exception);
                return new ListResult<StoryModel>();
            }
        }
    }
}