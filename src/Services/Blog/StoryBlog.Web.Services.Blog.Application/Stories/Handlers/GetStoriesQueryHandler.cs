using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Stories.Handlers.Helpers;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Author = StoryBlog.Web.Services.Blog.Application.Models.Author;
using Story = StoryBlog.Web.Services.Blog.Application.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesQueryHandler : IRequestHandler<GetStoriesQuery, StoriesQueryResult>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GetStoriesQuery> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public GetStoriesQueryHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ILogger<GetStoriesQuery> logger)
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
        public async Task<StoriesQueryResult> Handle(GetStoriesQuery request, CancellationToken cancellationToken)
        {
            if (null == request)
            {
                throw new ArgumentNullException(nameof(request));
            }

            logger.LogDebug($"Executing request: {request.GetType().Name}");

            var queryable = context.Stories.AsNoTracking();

            if (request.User.Identity.IsAuthenticated)
            {
                var authorId = 0;
                queryable = queryable.Where(story => story.Status == StoryStatus.Published || story.AuthorId == authorId);
            }
            else
            {
                queryable = queryable.Where(story => story.Status == StoryStatus.Published && story.IsPublic);
            }

            var id = queryable
                .OrderBy(story => story.Id)
                .Select(story => story.Id)
                .FirstOrDefault();

            switch (request.Cursor.Direction)
            {
                case NavigationCursorDirection.Backward:
                {
                    queryable = queryable
                        .Where(story => story.Id < request.Cursor.Id)
                        .OrderByDescending(story => story.Id);
                    break;
                }

                case NavigationCursorDirection.Forward:
                {
                    queryable = queryable
                        .Where(story => story.Id > request.Cursor.Id)
                        .OrderBy(story => story.Id);
                    break;
                }
            }

            queryable = queryable.Take(request.Cursor.Count);

            if (request.IncludeAuthors)
            {
                queryable = queryable.Include(story => story.Author);
            }

            if (request.IncludeComments)
            {
                if (request.IncludeAuthors)
                {
                    queryable = queryable.Include(story => story.Comments).ThenInclude(comment => comment.Author);
                }
                else
                {
                    queryable = queryable.Include(story => story.Comments);
                }
            }

            var entities = await queryable.ToListAsync(cancellationToken);
            var stories = new Collection<Story>();
            var authors = new Collection<Author>();

            entities.Sort(new StoryComparer());

            AuthorResourcesHelper.CreateMappedStories(mapper, stories, authors, entities, request.IncludeAuthors);

            return new StoriesQueryResult(
                stories,
                authors,
                backward: GetBackwardCursor(id, stories, request.Cursor.Count),
                forward: GetForwardCursor(stories, request.Cursor.Count)
            );
        }

        private static NavigationCursor GetBackwardCursor(long? minId, IList<Story> stories, int pageSize)
        {
            if (0 == stories.Count || false == minId.HasValue)
            {
                return null;
            }

            var id = stories[0].Id;

            if (minId.Value >= id)
            {
                return null;
            }

            return NavigationCursor.Backward(id, pageSize);
        }

        private static NavigationCursor GetForwardCursor(IList<Story> stories, int pageSize)
        {
            if (0 == stories.Count)
            {
                return null;
            }

            if (pageSize > stories.Count)
            {
                return null;
            }

            var id = stories[stories.Count - 1].Id;

            return NavigationCursor.Forward(id, pageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        private class StoryComparer : IComparer<Persistence.Models.Story>
        {
            public int Compare(Persistence.Models.Story x, Persistence.Models.Story y) =>
                x.Id == y.Id ? 0 : (x.Id > y.Id ? 1 : -1);
        }
    }
}