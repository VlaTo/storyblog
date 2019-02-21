using StoryBlog.Web.Services.Blog.Common;
using StoryBlog.Web.Services.Blog.Common.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
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

            try
            {
                using (var response = await client.GetAsync(requestUri, CancellationToken.None))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    return Json.Deserialize<ListResult<StoryModel>>(json);
                }
            }
            catch (HttpRequestException exception)
            {
                Debug.WriteLine(exception);
                return new ListResult<StoryModel>();
            }
        }

        public async Task CreateStoryAsync(StoryModel story)
        {
            var requestUri = new Uri(baseUri, "stories");

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await client.SendAsync(request, CancellationToken.None))
                {
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (HttpRequestException exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}