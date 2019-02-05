using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Story = StoryBlog.Web.Services.Blog.Application.Stories.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListQueryHandler : IRequestHandler<GetStoriesListQuery, IPagedQueryResult<Story>>
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
        public async Task<IPagedQueryResult<Story>> Handle(GetStoriesListQuery request, CancellationToken cancellationToken)
        {
            if (null == request)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var queryable = context.Stories.AsNoTracking();

            queryable = queryable
                .OrderBy(story => story.Id)
                .Where(story => story.Status == StoryStatus.Published && story.IsPublic);
            var id = queryable.Select(story => story.Id).FirstOrDefault();

            switch (request.Cursor.Direction)
            {
                case NavigationCursorDirection.Backward:
                {
                    //stories = stories.SkipWhile(story => story.Id < request.Cursor.Id);
                    queryable = queryable.Where(story => story.Id < request.Cursor.Id);
                    break;
                }

                case NavigationCursorDirection.Forward:
                {
                    queryable = queryable.Where(story => story.Id > request.Cursor.Id);
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

            var stories = await queryable
                .Take(request.Cursor.Count)
                .Select(story => mapper.Map<Story>(story))
                .ToListAsync(cancellationToken);

            return PagedQueryResult<Story>.Success(
                stories,
                GetBackwardCursor(id, stories, request.Cursor.Count),
                GetForwardCursor(stories, request.Cursor.Count)
            );
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