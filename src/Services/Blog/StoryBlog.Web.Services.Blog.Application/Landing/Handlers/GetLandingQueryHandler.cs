using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Landing.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Blog.Application.Landing.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using Author = StoryBlog.Web.Services.Blog.Application.Stories.Models.Author;

namespace StoryBlog.Web.Services.Blog.Application.Landing.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetLandingQueryHandler : IRequestHandler<GetLandingQuery, IRequestResult<Models.Landing>>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GetLandingQueryHandler> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public GetLandingQueryHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ILogger<GetLandingQueryHandler> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IRequestResult<Models.Landing>> Handle(GetLandingQuery request, CancellationToken cancellationToken)
        {
            if (null == request)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var authenticated = request.User.Identity.IsAuthenticated;
                var queryable = context.Settings.AsNoTracking();
                var landing = new Models.Landing
                {
                    Title = await GetStringValueAsync(queryable.Where(story => story.Name == "Landing.Title")),
                    Description = await GetStringValueAsync(queryable.Where(story => story.Name == "Landing.Description")),
                    HeroStory = await GetHeroStoryAsync(request.IncludeHeroStory, authenticated)
                };

                FillFeaturedStories(landing.FeaturedStories, request.FeaturedStoriesCount, authenticated);
                FillStoriesFeed(landing.FeedStories, request.StoriesFeedCount, authenticated);

                return RequestResult.Success(landing);
            }
            catch (Exception exception)
            {
                return RequestResult.Error<Models.Landing>(exception);
            }
        }

        private async Task<HeroStory> GetHeroStoryAsync(bool include, bool authenticated)
        {
            if (false == include)
            {
                return null;
            }

            var stories = context.Stories
                .OrderBy(story => story.Id)
                .Where(story => story.Status == StoryStatus.Published && (authenticated || story.IsPublic))
                .Select(story => new HeroStory(story.Id)
                {
                    Title = story.Title,
                    Slug = story.Slug,
                    Content = story.Content,
                    Created = story.Created,
                    Modified = story.Modified,
                    Author = mapper.Map<Author>(story.Author),
                    CommentsCount = story.Comments.Count(
                        comment => comment.Status == CommentStatus.Published && (authenticated || comment.IsPublic)
                    )
                });

            return await stories.FirstOrDefaultAsync();
        }

        private void FillFeaturedStories(ICollection<FeedStory> collection, int count, bool authenticated)
        {
            if (0 == count)
            {
                return;
            }

            var stories = context.Stories
                .AsNoTracking()
                .Include(story => story.Author)
                .Include(story => story.Comments)
                .OrderBy(story => story.Id)
                .Where(story => story.Status == StoryStatus.Published && (authenticated || story.IsPublic))
                .Select(story => new FeedStory(story.Id)
                {
                    Title = story.Title,
                    Slug = story.Slug,
                    Content = TrimContent(story.Content),
                    Created = story.Created,
                    Modified = story.Modified,
                    Author = mapper.Map<Author>(story.Author),
                    CommentsCount = story.Comments.Count(
                        comment => comment.Status == CommentStatus.Published && (authenticated || comment.IsPublic)
                    )
                })
                .Take(count);

            foreach (var story in stories)
            {
                collection.Add(mapper.Map<FeedStory>(story));
            }
        }

        private void FillStoriesFeed(ICollection<FeedStory> collection, int count, bool authenticated)
        {
            if (0 == count)
            {
                return;
            }

            var stories = context.Stories
                .AsNoTracking()
                .Include(story => story.Author)
                .Include(story => story.Comments)
                .OrderBy(story => story.Id)
                .Where(story => story.Status == StoryStatus.Published && (authenticated || story.IsPublic))
                .Select(story => new FeedStory(story.Id)
                {
                    Title = story.Title,
                    Slug = story.Slug,
                    Content = story.Content,
                    Created = story.Created,
                    Modified = story.Modified,
                    Author = mapper.Map<Author>(story.Author),
                    CommentsCount = story.Comments.Count(
                        comment => comment.Status == CommentStatus.Published && (authenticated || comment.IsPublic)
                    )
                })
                .Take(count);

            foreach (var story in stories)
            {
                collection.Add(mapper.Map<FeedStory>(story));
            }
        }

        private static Task<string> GetStringValueAsync(IQueryable<Settings> queryable)
        {
            var value = queryable.Select(story => story.Value).FirstOrDefault();

            if (null == value || 0 == value.Length)
            {
                return Task.FromResult(String.Empty);
            }

            return Task.FromResult(Encoding.UTF8.GetString(value));
        }

        private static string TrimContent(string content)
        {
            var words = content.Split(' ').Take(20);
            return String.Join(' ', words);
        }
    }
}
