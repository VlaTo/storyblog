using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using StoryBlog.Web.Services.Blog.Interop;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class BlogApiClient : IBlogApiClient
    {
        private readonly HttpClient client;
        private readonly Uri baseUri = new Uri("http://localhost:3000/api/v1/");
        private readonly ILogger logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public BlogApiClient(HttpClient client, ILogger<IBlogApiClient> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        /// <inheritdoc cref="IBlogApiClient.GetStoriesAsync" />
        public async Task<LandingModel> GetLandingAsync(LandingIncludes flags)
        {
            var path = new Uri(baseUri, "landing");
            var include = EnumFlags.ToQueryString(flags);
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) {Query = query.ToUriComponent()}.Uri;

            try
            {
                logger.LogDebug($"[{nameof(BlogApiClient)}] Requesting landing from \"{requestUri}\"");

                using (var response = await client.GetAsync(requestUri, CancellationToken.None))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var data = Json.Deserialize<LandingModel>(json);

                    logger.LogDebug($"[{nameof(BlogApiClient)}] Fetch landing status: {response.StatusCode}");

                    return data;
                }
            }
            catch (HttpRequestException exception)
            {
                logger.LogError(exception, $"Failed to fetch stories from \"{requestUri}\"");
                return new LandingModel();
            }
        }

        /// <inheritdoc cref="IBlogApiClient.GetStoriesAsync" />
        public async Task<ListResult<StoryModel>> GetStoriesAsync(StoryIncludes flags)
        {
            var path = new Uri(baseUri, "stories");
            var include = EnumFlags.ToQueryString(flags);
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) {Query = query.ToUriComponent()}.Uri;

            try
            {
                logger.LogDebug($"[{nameof(BlogApiClient)}] Requesting stories from \"{requestUri}\"");

                using (var response = await client.GetAsync(requestUri, CancellationToken.None))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var data = Json.Deserialize<ListResult<StoryModel>>(json);

                    logger.LogDebug($"[{nameof(BlogApiClient)}] Stories fetch status {response.StatusCode}");

                    return data;
                }
            }
            catch (HttpRequestException exception)
            {
                logger.LogError(exception, $"Failed to fetch stories from \"{requestUri}\"");
                return new ListResult<StoryModel>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public async Task<StoryModel> GetStoryAsync(string slug, StoryIncludes flags)
        {
            var path = new Uri(baseUri, $"story/{slug}");
            var include = EnumFlags.ToQueryString(flags);
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) { Query = query.ToUriComponent() }.Uri;

            try
            {
                logger.LogDebug($"[{nameof(BlogApiClient)}] Requesting stories from \"{requestUri}\"");

                using (var response = await client.GetAsync(requestUri, CancellationToken.None))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var data = Json.Deserialize<StoryModel>(json);

                    logger.LogDebug($"[{nameof(BlogApiClient)}] Stories fetch status {response.StatusCode}");

                    return data;
                }
            }
            catch (HttpRequestException exception)
            {
                logger.LogError(exception, $"Failed to fetch stories from \"{requestUri}\"");
                return null;
            }
        }
    }
}