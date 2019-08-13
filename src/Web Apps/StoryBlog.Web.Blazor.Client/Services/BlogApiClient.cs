using StoryBlog.Web.Blazor.Client.Store.Models;
using StoryBlog.Web.Services.Blog.Interop.Core;
using StoryBlog.Web.Services.Blog.Interop.Includes;
using StoryBlog.Web.Services.Blog.Interop.Models;
using StoryBlog.Web.Services.Shared.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Blazor.Client.Store.Effects;

namespace StoryBlog.Web.Blazor.Client.Services
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
        public BlogApiClient(HttpClient client, BlogApiOptions options)
        {
            this.client = client;
            this.options = options;
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
        public async Task<Story> GetStoryAsync(string slug, StoryFlags flags, CancellationToken cancellationToken)
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
        public async Task<Comment> CreateCommentAsync(string slug, long? parentId, string message, CancellationToken cancellationToken)
        {
            var requestUri = new Uri(options.Host, "comments");

            try
            {
                using (var request = new HttpRequestMessage(HttpMethod.Post, requestUri))
                {
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(JsonMediaType));

                    using (var response = await client.SendAsync(request, CancellationToken.None))
                    {
                        var temp = response.EnsureSuccessStatusCode();

                        temp.Content
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

        private static Story ProcessStoryActionResult(GetStoryActionModel result)
        {
            var authors = CreateAuthors(result.Meta.Resources.Authors);
            return new Story
            {
                Title = result.Data.Title,
                Slug = result.Data.Slug,
                Author = authors[result.Data.Author],
                Content = result.Data.Content,
                Published = GetPublishedDate(result.Data.Published, result.Data.Created),
                IsCommentsClosed = result.Data.Closed,
                Comments = CreateCommentsThree(result.Data.Comments, authors)
            };
        }

        private static ICollection<Comment> CreateCommentsThree(ICollection<CommentModel> plainComments, IReadOnlyDictionary<int, Author> authors)
        {
            void CreateChildComments(long parentId, Comment parent)
            {
                foreach (var source in plainComments.Where(comment => parentId == comment.Parent))
                {
                    var comment = new Comment(parent)
                    {
                        Id = source.Id,
                        Content = source.Content,
                        Author = authors[source.Author],
                        Published = GetPublishedDate(source.Modified, source.Created)
                    };

                    parent.Comments.Add(comment);

                    CreateChildComments(source.Id, comment);
                }
            }

            var comments = new Collection<Comment>();

            foreach (var source in plainComments.Where(comment => null == comment.Parent))
            {
                var comment = new Comment(null)
                {
                    Id = source.Id,
                    Content = source.Content,
                    Author = authors[source.Author],
                    Published = GetPublishedDate(source.Modified, source.Created)
                };

                comments.Add(comment);
                CreateChildComments(source.Id, comment);
            }

            return comments;
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