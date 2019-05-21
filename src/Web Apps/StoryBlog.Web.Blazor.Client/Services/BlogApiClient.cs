using Microsoft.JSInterop;
using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Blazor.Reactive;
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

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class BlogApiClient : IBlogApiClient, IDisposable
    {
        private readonly HttpClient client;
        private readonly Uri baseUri = new Uri("http://localhost:3000/api/v1/");
        private AuthorizationToken authorizationToken;
        private readonly IDisposable disposable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="authorizationContext"></param>
        public BlogApiClient(HttpClient client, IObservable<AuthorizationToken> authorizationContext)
        {
            this.client = client;

            disposable = authorizationContext.Subscribe(value =>
            {
                authorizationToken = value;
            });
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
                    client.SetBearerToken(authorizationToken.Payload);
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
        public async Task<IEnumerable<FeedStory>> GetStoriesAsync(StoryIncludes flags)
        {
            var path = new Uri(baseUri, "stories");
            var include = EnumFlags.ToQueryString(flags);
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) {Query = query.ToUriComponent()}.Uri;

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = requestUri
                };

                if (null != authorizationToken)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(
                        authorizationToken.Scheme,
                        authorizationToken.Payload
                    );
                }

                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json", 1.0d));

                using (var response = await client.SendAsync(request))
                {
                    var message = response.EnsureSuccessStatusCode();
                    var json = await message.Content.ReadAsStringAsync();
                    var data = Json.Deserialize<ListResult<StoryModel, ResourcesMetaInfo<AuthorsResource>>>(json);

                    return ProcessResult(data);
                }
            }
            catch (HttpRequestException)
            {
                return Enumerable.Empty<FeedStory>();
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

        private static IEnumerable<FeedStory> ProcessResult(ListResult<StoryModel, ResourcesMetaInfo<AuthorsResource>> result)
        {
            var authors = GetAuthorIndex(result.Meta.Resources.Authors);

            return result.Data.Select(story => new FeedStory
            {
                Title = story.Title,
                Slug = story.Slug,
                Author = authors[story.Author],
                Content = story.Content,
                Published = GetPublishedDate(story.Published, story.Created),
                CommentsCount = story.Comments.Length
            });
        }

        private static IReadOnlyDictionary<int, Author> GetAuthorIndex(IEnumerable<AuthorModel> authors)
        {
            var dictionary = new Dictionary<int, Author>();
            var index = 0;

            foreach (var author in authors)
            {
                dictionary[index++] = new Author
                {
                    Name = author.Name
                };
            }

            return dictionary;
        }

        private static DateTime GetPublishedDate(DateTimeOffset? source, DateTimeOffset fallback)
        {
            var value = source.GetValueOrDefault(fallback);
            return value.ToLocalTime().DateTime;
        }
    }
}