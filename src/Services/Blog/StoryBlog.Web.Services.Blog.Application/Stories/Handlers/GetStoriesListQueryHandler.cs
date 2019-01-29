using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using Story = StoryBlog.Web.Services.Blog.Application.Stories.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListQueryHandler : IRequestHandler<GetStoriesListQuery, IReadOnlyCollection<Story>>
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
        public async Task<IReadOnlyCollection<Story>> Handle(
            GetStoriesListQuery request,
            CancellationToken cancellationToken)
        {
            logger.LogDebug("{Name}", request.User.Identity.Name);

            var models = await context.Stories
                .AsNoTracking()
                .OrderBy(story => story.Id)
                .Where(story => story.Status == StoryStatus.Published && story.IsPublic)
                .Skip(0)
                .Take(10)
                .Select(story => mapper.Map<Story>(story))
                .ToListAsync(cancellationToken);

            return new ReadOnlyCollection<Story>(models);
        }
    }
}