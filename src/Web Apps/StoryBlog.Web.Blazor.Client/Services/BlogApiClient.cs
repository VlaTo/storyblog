using Microsoft.Extensions.Options;
using StoryBlog.Web.Client.Models;
using StoryBlog.Web.Client.Store.Models;
using StoryBlog.Web.Client.Store.Models.Data;
using StoryBlog.Web.Services.Blog.Interop.Core;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Comment = StoryBlog.Web.Client.Store.Models.Data.Comment;

namespace StoryBlog.Web.Client.Services
{
    internal sealed class BlogApiClient : IBlogApiClient, IDisposable
    {
        private const string JsonMediaType = "application/json";

        private readonly HttpClient client;
        private readonly BlogApiOptions options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="options"></param>
        public BlogApiClient(
            HttpClient client,
            IOptions<BlogApiOptions> options)
        {
            this.client = client;
            this.options = options.Value;
        }

        /// <inheritdoc cref="IBlogApiClient.GetStoriesAsync" />
        /*public async Task<LandingModel> GetLandingAsync(LandingIncludes flags, CancellationToken cancellationToken)
        {
            var path = new Uri(baseUri, "landing");
            var include = EnumFlags.ToQueryString(flags);
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) {Query = query.ToUriComponent()}.Uri;

            try
            {
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
        }*/

        /// <inheritdoc cref="IBlogApiClient.GetStoriesAsync(StoryFlags, CancellationToken)" />
        public Task<EntityListResult<FeedStory>> GetStoriesAsync(StoryFlags flags, CancellationToken cancellationToken)
        {
            var path = new Uri(options.Host, "stories");
            var include = Flags.Format(typeof(StoryFlags), flags, "F");
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) {Query = query.ToUriComponent()}.Uri;

            return GetStoriesInternalAsync(requestUri, cancellationToken);
        }

        /// <inheritdoc cref="IBlogApiClient.GetStoriesAsync(Uri, CancellationToken)" />
        public Task<EntityListResult<FeedStory>> GetStoriesAsync(Uri requestUri, CancellationToken cancellationToken)
        {
            if (null == requestUri)
            {
                throw new ArgumentNullException(nameof(requestUri));
            }

            var path = new Uri(options.Host, requestUri);

            return GetStoriesInternalAsync(path, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public async Task<StoryResult> GetStoryAsync(string slug, StoryFlags flags, CancellationToken cancellationToken)
        {
            var path = new Uri(options.Host, $"story/{slug}");
            var include = Flags.Format(typeof(StoryFlags), flags, "F");
            var query = QueryString.Create(nameof(include), include);
            var requestUri = new UriBuilder(path) { Query = query.ToUriComponent() }.Uri;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri
            };

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

            try
            {
                using (var response = await client.SendAsync(request, cancellationToken))
                {
                    var httpResponse = response.EnsureSuccessStatusCode();

                    using (var stream = await httpResponse.Content.ReadAsStreamAsync())
                    {
                        var result = await JsonSerializer.DeserializeAsync<GetStoryActionModel>(
                            stream,
                            cancellationToken: cancellationToken
                        );
                        return ProcessStoryActionResult(result);
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
            var requestUri = new Uri(options.Host, "stories");

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

        /// <inheritdoc cref="IBlogApiClient.CreateCommentAsync" />
        public async Task<CommentCreatedResult> CreateCommentAsync(string slug, long? parentId, string text, CancellationToken cancellationToken)
        {
            var path = $"comments/{slug}";

            if (parentId.HasValue)
            {
                path += ("/" + parentId.Value);
            }

            var requestUri = new Uri(options.Host, path);

            try
            {
                CommentCreatedResult comment;

                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    request.Method = HttpMethod.Post;
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));
                    request.Content = await CreateCommentPostContentAsync(text, true, cancellationToken);

                    using (var response = await client.SendAsync(request, CancellationToken.None))
                    {
                        var message = response.EnsureSuccessStatusCode();

                        using (var stream = await message.Content.ReadAsStreamAsync())
                        {
                            var result = await JsonSerializer.DeserializeAsync<CommentCreatedModel>(
                                stream,
                                cancellationToken: cancellationToken
                            );

                            comment = new CommentCreatedResult
                            {
                                Id = result.Id,
                                Content = result.Content,
                                Parent = result.Parent,
                                Author = new Author(result.Author.Name),
                                Published = GetPublishedDate(result.Modified, result.Created)
                            };
                        }
                    }
                }

                return comment;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        /// <inheritdoc cref="IBlogApiClient.GetRubricsAsync" />
        /*public async Task<IEnumerable<RubricModel>> GetRubricsAsync(CancellationToken cancellationToken)
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
        }*/

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
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

                    using (var response = await client.SendAsync(request, cancellationToken))
                    {
                        var message = response.EnsureSuccessStatusCode();

                        using (var stream = await message.Content.ReadAsStreamAsync())
                        {
                            var data = await JsonSerializer.DeserializeAsync<GetStoriesActionModel>(
                                stream,
                                cancellationToken: cancellationToken
                            );

                            return ProcessStoriesActionResult(data);
                        }
                    }
                }
            }
            catch (HttpRequestException)
            {
                return new EntityListResult<FeedStory>(Enumerable.Empty<FeedStory>());
            }
        }

        private static async Task<HttpContent> CreateCommentPostContentAsync(string text, bool isPublic, CancellationToken cancellationToken)
        {
            var model = new CreateCommentModel
            {
                Content = text,
                IsPublic = isPublic
            };

            var encoding = Encoding.UTF8;

            using (var stream = new MemoryStream())
            {
                await JsonSerializer.SerializeAsync(stream, model, cancellationToken: cancellationToken);

                var bytes = stream.ToArray();
                var content = encoding.GetString(bytes);

                return new StringContent(content, encoding, JsonMediaType);
            }
        }

        private static StoryResult ProcessStoryActionResult(GetStoryActionModel result)
        {
            var authors = CreateAuthors(result.Meta.Resources.Authors);

            ICollection<Comment> CreateComments(IEnumerable<CommentModel> comments)
            {
                var collection = new Collection<Comment>();

                foreach (var comment in comments)
                {
                    collection.Add(new Comment
                    {
                        Id = comment.Id,
                        ParentId = comment.Parent,
                        Author = authors[comment.Author],
                        Content = comment.Content,
                        Published = GetPublishedDate(comment.Modified, comment.Created)
                    });
                }

                return collection;
            }

            return new StoryResult
            {
                Title = result.Data.Title,
                Slug = result.Data.Slug,
                Author = authors[result.Data.Author],
                Content = result.Data.Content,
                Published = GetPublishedDate(result.Data.Published, result.Data.Created),
                IsCommentsClosed = result.Data.Closed,
                Comments = CreateComments(result.Data.Comments)
            };
        }

        private static EntityListResult<FeedStory> ProcessStoriesActionResult(GetStoriesActionModel result)
        {
            var authors = CreateAuthors(result.Meta.Resources.Authors);

            return new EntityListResult<FeedStory>(
                result.Data.Select(story => new FeedStory
                {
                    Title = story.Title,
                    Slug = story.Slug,
                    Author = authors[story.Author],
                    Content = story.Content,
                    Published = GetPublishedDate(story.Published, story.Created),
                    CommentsCount = story.Comments.Count
                }),
                GetNavigationUri(result.Meta.Navigation.Previous),
                GetNavigationUri(result.Meta.Navigation.Next)
            );
        }

        private static IReadOnlyDictionary<int, Author> CreateAuthors(IEnumerable<AuthorModel> authors)
        {
            var dictionary = new Dictionary<int, Author>();
            var index = 0;

            foreach (var author in authors)
            {
                dictionary[index++] = new Author(author.Name);
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