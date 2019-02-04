using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using Story = StoryBlog.Web.Services.Blog.Application.Stories.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryQueryHandler : IRequestHandler<GetStoryQuery, RequestResult<Story>>
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
        public GetStoryQueryHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ILogger<GetStoriesListQuery> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle" />
        public async Task<RequestResult<Story>> Handle(GetStoryQuery request, CancellationToken cancellationToken)
        {
            logger.LogDebug("{Name}", request.User.Identity.Name);

            var authenticated = request.User.Identity.IsAuthenticated;
            var model = await context.Stories
                .AsNoTracking()
                .Where(story => (authenticated || story.IsPublic) && story.Slug == request.Slug)
                .FirstOrDefaultAsync(cancellationToken);

            return RequestResult.Success(mapper.Map<Story>(model));
        }
    }
}