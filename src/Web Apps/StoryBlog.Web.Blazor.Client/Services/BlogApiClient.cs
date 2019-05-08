using Microsoft.JSInterop;
using StoryBlog.Web.Services.Blog.Interop;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Blazor.Reactive;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class BlogApiClient : IBlogApiClient, IDisposable
    {
        private readonly HttpClient client;
        private readonly AuthorizationContext authorizationContext;
        private readonly Uri baseUri = new Uri("http://localhost:3000/api/v1/");
        private AuthorizationToken authorizationToken;
        private readonly IDisposable disposable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="authorizationContext"></param>
        public BlogApiClient(HttpClient client, AuthorizationContext authorizationContext)
        {
            this.client = client;
            this.authorizationContext = authorizationContext;
            disposable = authorizationContext.Subscribe(value => authorizationToken = value);
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
                if (null != authorizationToken)
                {
                    client.SetBearerToken(authorizationToken.Token);
                }

                using (var response = await client.GetAsync(requestUri, CancellationToken.None))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var data = Json.Deserialize<LandingModel>(json);

                    return data;
                }
            }
            catch (HttpRequestException)
            {
                return new LandingModel();
            }
        }

        /// <inheritdoc cref="IBlogApiClient.GetStoriesAsync" />
        public async Task<ListResult<StoryModel, ResourcesMetaInfo<AuthorsResource>>> GetStoriesAsync(StoryIncludes flags)
        {
            var path = new Uri(baseUri, "stories");
            var include = EnumFlags.ToQueryString(flags);
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) {Query = query.ToUriComponent()}.Uri;

            try
            {
                using (var response = await client.GetAsync(requestUri, CancellationToken.None))
                {
                    var message = response.EnsureSuccessStatusCode();
                    var json = await message.Content.ReadAsStringAsync();
                    var data = Json.Deserialize<ListResult<StoryModel, ResourcesMetaInfo<AuthorsResource>>>(json);

                    return data;
                }
            }
            catch (HttpRequestException)
            {
                return new ListResult<StoryModel, ResourcesMetaInfo<AuthorsResource>>();
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
                using (var response = await client.GetAsync(requestUri, CancellationToken.None))
                {
                    response.EnsureSuccessStatusCode();

                    var json = await response.Content.ReadAsStringAsync();
                    var data = Json.Deserialize<StoryModel>(json);

                    return data;
                }
            }
            catch (HttpRequestException exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="story"></param>
        /// <returns></returns>
        public async Task<bool> CreateStoryAsync(StoryModel story)
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

                return true;
            }
            catch (HttpRequestException exception)
            {
                return false;
            }
        }

        /// <inheritdoc cref="IBlogApiClient.GetRubricsAsync" />
        public async Task<IEnumerable<RubricModel>> GetRubricsAsync()
        {
            var requestUri = new Uri(baseUri, "rubrics");

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await client.SendAsync(request, CancellationToken.None))
                {
                    var message = response.EnsureSuccessStatusCode();
                    var json = await message.Content.ReadAsStringAsync();
                    var result = Json.Deserialize<ListResult<RubricModel, ResultMetaInfo>>(json);

                    return result.Data ?? Enumerable.Empty<RubricModel>();
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public void Dispose()
        {
            disposable.Dispose();
        }
    }
}