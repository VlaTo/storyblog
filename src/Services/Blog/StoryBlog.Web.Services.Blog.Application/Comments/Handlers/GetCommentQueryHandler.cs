using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Comments.Queries;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, IRequestResult<Comment>>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GetCommentQuery> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public GetCommentQueryHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ILogger<GetCommentQuery> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IRequestResult<Comment>> Handle(GetCommentQuery request, CancellationToken cancellationToken)
        {
            var authenticated = request.User.Identity.IsAuthenticated;
            var queryable = context.Comments.AsNoTracking();

            if (request.IncludeAuthor)
            {
                queryable = queryable.Include(entity => entity.Author);
            }

            var comment = await queryable
                .Where(entity => (authenticated || entity.IsPublic) && entity.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (null == comment)
            {
                return null;
            }

            return RequestResult.Success(mapper.Map<Comment>(comment));
        }
    }
}