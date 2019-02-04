using System;
using System.Collections.Generic;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using Story = StoryBlog.Web.Services.Blog.Application.Stories.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListQueryHandler : IRequestHandler<GetStoriesListQuery, PagedQueryResult<Story>>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GetStoriesListQuery> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public GetStoriesListQueryHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ILogger<GetStoriesListQuery> logger)
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
        public async Task<PagedQueryResult<Story>> Handle(
            GetStoriesListQuery request,
            CancellationToken cancellationToken)
        {
            if (null == request)
            {
                throw new ArgumentNullException(nameof(request));
            }

            logger.LogDebug("{Name}", request.User.Identity.Name);

            var stories = context.Stories.AsNoTracking();

            stories = stories
                .OrderBy(story => story.Id)
                .Where(story => story.Status == StoryStatus.Published && story.IsPublic);
            var id = stories.Select(story => story.Id).FirstOrDefault();

            switch (request.Cursor.Direction)
            {
                case NavigationCursorDirection.Backward:
                {
                    //stories = stories.SkipWhile(story => story.Id < request.Cursor.Id);
                    stories = stories.Where(story => story.Id < request.Cursor.Id);
                    break;
                }

                case NavigationCursorDirection.Forward:
                {
                    stories = stories.Where(story => story.Id > request.Cursor.Id);
                    break;
                }
            }

            stories = stories.Take(request.Cursor.Count);

            if (request.IncludeAuthors)
            {
                stories = stories.Include(story => story.Author);
            }

            if (request.IncludeComments)
            {
                if (request.IncludeAuthors)
                {
                    stories = stories.Include(story => story.Comments).ThenInclude(comment => comment.Author);
                }
                else
                {
                    stories = stories.Include(story => story.Comments);
                }
            }

            var entities = await stories
                .Take(request.Cursor.Count)
                .Select(story => mapper.Map<Story>(story))
                .ToListAsync(cancellationToken);

            var result = PagedQueryResult<Story>.FromCollection(entities);

            result.Backward = GetBackwardCursor(id, entities, request.Cursor.Count);
            result.Forward = GetForwardCursor(entities, request.Cursor.Count);

            return result;
        }

        private static NavigationCursor GetBackwardCursor(long? firstId, IList<Story> stories, int pageSize)
        {
            if (0 == stories.Count)
            {
                return null;
            }

            var id = stories[0].Id;

            if (firstId >= id)
            {
                return null;
            }

            return NavigationCursor.Backward(id - pageSize, pageSize);
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
    }
}