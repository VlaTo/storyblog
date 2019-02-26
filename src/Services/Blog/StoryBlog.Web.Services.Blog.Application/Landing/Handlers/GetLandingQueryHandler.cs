using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Landing.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
                    Title = await GetStringValueAsync(queryable.Where(story => story.Name == "Title")),
                    Description = await GetStringValueAsync(queryable.Where(story => story.Name == "Description"))
                };

                if (request.IncludeHeroStory)
                {
                    var stories = context.Stories
                        .OrderBy(story => story.Id)
                        .Where(story => story.Status == StoryStatus.Published && (authenticated || story.IsPublic))
                        .Select(story => new FeedStory(story.Id)
                        {
                            Title = story.Title,
                            Slug = story.Slug,
                            Content = story.Content,
                            IsPublic = story.IsPublic,
                            Created = story.Created,
                            Modified = story.Modified,
                            Author = mapper.Map<Author>(story.Author),
                            CommentsCount = story.Comments.Count(
                                comment => comment.Status == CommentStatus.Published && (authenticated || comment.IsPublic)
                            )
                        });
                    landing.HeroStory = stories.FirstOrDefault();
                }

                if (0 < request.FeaturedStoriesCount)
                {
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
                            IsPublic = story.IsPublic,
                            Created = story.Created,
                            Modified = story.Modified,
                            Author = mapper.Map<Author>(story.Author),
                            CommentsCount = story.Comments.Count(
                                comment => comment.Status == CommentStatus.Published && (authenticated || comment.IsPublic)
                            )
                        })
                        .Take(request.FeaturedStoriesCount);

                    foreach (var story in stories)
                    {
                        landing.FeaturedStories.Add(mapper.Map<FeedStory>(story));
                    }
                }

                if (0 < request.StoriesFeedCount)
                {
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
                            IsPublic = story.IsPublic,
                            Created = story.Created,
                            Modified = story.Modified,
                            Author = mapper.Map<Author>(story.Author),
                            CommentsCount = story.Comments.Count(
                                comment => comment.Status == CommentStatus.Published && (authenticated || comment.IsPublic)
                            )
                        })
                        .Take(request.StoriesFeedCount);

                    foreach (var story in stories)
                    {
                        landing.StoriesFeed.Add(mapper.Map<FeedStory>(story));
                    }
                }

                return RequestResult.Success(landing);
            }
            catch (Exception exception)
            {
                return RequestResult.Error<Models.Landing>(exception);
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
    }
}
