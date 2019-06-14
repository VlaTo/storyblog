﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Story = StoryBlog.Web.Services.Blog.Application.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoryQueryHandler : IRequestHandler<GetStoryQuery, StoryRequestResult>
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
        public GetStoryQueryHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ILogger<GetStoriesQuery> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle" />
        public async Task<StoryRequestResult> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            var authenticated = request.User.Identity.IsAuthenticated;
            var queryable = context.Stories.AsNoTracking();

            if (request.IncludeAuthors)
            {
                if (request.IncludeComments)
                {
                    queryable = queryable
                        .Include(entity => entity.Comments)
                        .ThenInclude(entity => entity.Author);
                }

                queryable = queryable.Include(entity => entity.Author);
            }

            if (request.IncludeComments)
            {
                queryable = queryable.Include(entity => entity.Comments);
            }

            var story = await queryable
                .Where(entity => (authenticated || entity.IsPublic) && entity.Slug == request.Slug)
                .FirstOrDefaultAsync(cancellationToken);

            if (null == story)
            {
                return null;
            }

            return new StoryRequestResult(mapper.Map<Story>(story), null);
        }
    }
}