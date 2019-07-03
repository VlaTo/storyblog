using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Blazor.Client.Services
{
    internal sealed class BlogApiClient : IBlogApiClient, IDisposable
    {
        private const string JsonMediaType = "application/json";

        private readonly HttpClient client;
        private readonly Uri baseUri = new Uri("http://localhost:3000/api/v1/");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public BlogApiClient(HttpClient client)
        {
            this.client = client;
        }

        /// <inheritdoc cref="IBlogApiClient.GetStoriesAsync" />
        public async Task<LandingModel> GetLandingAsync(LandingIncludes flags, CancellationToken cancellationToken)
        {
            var path = new Uri(baseUri, "landing");
            var include = EnumFlags.ToQueryString(flags);
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) {Query = query.ToUriComponent()}.Uri;

            try
            {
                /*if (null != authorizationToken)
                {
                    client.SetBearerToken(authorizationToken.Payload);
                }*/

                using (var response = await client.GetAsync(requestUri, cancellationToken))
                {
                    var httpResponse = response.EnsureSuccessStatusCode();

                    using (var stream = await httpResponse.Content.ReadAsStreamAsync())
                    {
                        var result = await JsonSerializer.ReadAsync<LandingModel>(stream, cancellationToken: cancellationToken);
                        return result;
                    }
                }
            }
            catch (HttpRequestException)
            {
                return new LandingModel();
            }
        }

        /// <inheritdoc cref="IBlogApiClient.GetStoriesAsync" />
        public Task<EntityListResult<FeedStory>> GetStoriesAsync(StoryIncludes flags, CancellationToken cancellationToken)
        {
            var path = new Uri(baseUri, "stories");
            var include = EnumFlags.ToQueryString(flags);
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) {Query = query.ToUriComponent()}.Uri;

            return GetStoriesFromAsync(requestUri, cancellationToken);
        }

        /// <inheritdoc cref="IBlogApiClient.GetStoriesAsync(System.Uri)" />
        public Task<EntityListResult<FeedStory>> GetStoriesFromAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            if (null == requestUri)
            {
                throw new ArgumentNullException(nameof(requestUri));
            }

            var path = new Uri(baseUri, requestUri);

            return GetStoriesInternalAsync(path, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public async Task<Story> GetStoryAsync(string slug, StoryIncludes flags, CancellationToken cancellationToken)
        {
            const string mediaType = "application/json";

            var path = new Uri(baseUri, $"story/{slug}");
            var include = EnumFlags.ToQueryString(flags);
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) { Query = query.ToUriComponent() }.Uri;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri
            };

            /*authorizationOptions.AuthorizationToken
            if (null != authorizationToken)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(
                    authorizationToken.Scheme,
                    authorizationToken.Payload
                );
            }*/

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType, 1.0d));

            try
            {
                using (var response = await client.SendAsync(request, cancellationToken))
                {
                    var httpResponse = response.EnsureSuccessStatusCode();

                    using (var stream = await httpResponse.Content.ReadAsStreamAsync())
                    {
                        var result = await JsonSerializer.ReadAsync<StoryModel>(stream, cancellationToken: cancellationToken);
                        return ProcessResult(result);
                    }
                }
            }
            catch(HttpRequestException)
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
                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

                    using (var response = await client.SendAsync(request, CancellationToken.None))
                    {
                        response.EnsureSuccessStatusCode();
                    }

                    return true;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        /// <inheritdoc cref="IBlogApiClient.GetRubricsAsync" />
        public async Task<IEnumerable<RubricModel>> GetRubricsAsync(CancellationToken cancellationToken)
        {
            var requestUri = new Uri(baseUri, "rubrics");

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    using (var response = await client.SendAsync(request, cancellationToken))
                    {
                        var message = response.EnsureSuccessStatusCode();

                        using (var stream = await message.Content.ReadAsStreamAsync())
                        {
                            var result = await JsonSerializer.ReadAsync<ListResult<RubricModel, ResultMetaInfo>>(stream, cancellationToken: cancellationToken);
                            return result.Data ?? Enumerable.Empty<RubricModel>();
                        }
                    }
                }
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public void Dispose()
        {
            //authorizationMonitorToken.Dispose();
        }

        private async Task<EntityListResult<FeedStory>> GetStoriesInternalAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
                {
                    /*if (null != authorizationToken)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue(
                            authorizationToken.Scheme,
                            authorizationToken.Payload
                        );
                    }*/

                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType, 1.0d));

                    using (var response = await client.SendAsync(request, cancellationToken))
                    {
                        var message = response.EnsureSuccessStatusCode();

                        using (var stream = await message.Content.ReadAsStreamAsync())
                        {
                            var data = await JsonSerializer.ReadAsync<ListResult<StoryModel, ResourcesNavigationMetaInfo<AuthorsResource>>>(stream, cancellationToken: cancellationToken);
                            return ProcessResult(data);
                        }
                    }
                }
            }
            catch (HttpRequestException)
            {
                return new EntityListResult<FeedStory>(Enumerable.Empty<FeedStory>());
            }
        }

        private static Story ProcessResult(StoryModel result)
        {
            //return new EntityListResult<FeedStory>(Enumerable.Empty<FeedStory>());
            return null;
        }

        private static EntityListResult<FeedStory> ProcessResult(ListResult<StoryModel, ResourcesNavigationMetaInfo<AuthorsResource>> result)
        {
            var authors = GetAuthorIndex(result.Meta.Resources.Authors);

            return new EntityListResult<FeedStory>(
                result.Data.Select(story => new FeedStory
                {
                    Title = story.Title,
                    Slug = story.Slug,
                    Author = authors[story.Author],
                    Content = story.Content,
                    Published = GetPublishedDate(story.Published, story.Created),
                    CommentsCount = story.Comments.Length
                }),
                GetNavigationUri(result.Meta.Navigation.Previous),
                GetNavigationUri(result.Meta.Navigation.Next)
            );
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

        private static Uri GetNavigationUri(string source)
            => String.IsNullOrEmpty(source) ? null : new Uri(source, UriKind.Relative);
    }
}