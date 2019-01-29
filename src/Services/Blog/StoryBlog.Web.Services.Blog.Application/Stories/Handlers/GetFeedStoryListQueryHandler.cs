using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetFeedStoryListQueryHandler : IRequestHandler<GetFeedStoryListQuery, IReadOnlyCollection<StoryModel>>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GetFeedStoryListQuery> logger;

        public GetFeedStoryListQueryHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ILogger<GetFeedStoryListQuery> logger
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public Task<IReadOnlyCollection<StoryModel>> Handle(GetFeedStoryListQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogDebug("{Name}", request.User.Identity.Name);

            var models = context.Stories
                .Where(story => story.Status == StoryStatus.Published && story.IsPublic)
                .Skip(0)
                .Take(10)
                .Select(story => mapper.Map<StoryModel>(story))
                .ToList();

            return Task.FromResult<IReadOnlyCollection<StoryModel>>(
                new ReadOnlyCollection<StoryModel>(models)
            );
        }
    }
}