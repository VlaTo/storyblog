using StoryBlog.Web.Services.Blog.Common;
using StoryBlog.Web.Services.Blog.Common.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Blazor;
using Microsoft.Extensions.Logging;

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
            var path = baseUri + "stories";

            logger.LogDebug($"Call to API: {path}");

            var result = await client.GetJsonAsync<ListResult<StoryModel>>(path);

            logger.LogDebug($"Count: {result.Data.Count()}");

            return result;
        }
    }
}