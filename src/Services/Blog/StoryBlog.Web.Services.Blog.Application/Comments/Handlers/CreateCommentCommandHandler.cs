using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Services.Blog.Application.Comments.Commands;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Comment = StoryBlog.Web.Services.Blog.Application.Stories.Models.Comment;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, IRequestResult<Comment>>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// 
        /// </summary>
        public CreateCommentCommandHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider)
        {
            this.context = context;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle" />
        public async Task<IRequestResult<Comment>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            //var name = request.User.Identity.Name;
            var author = await context.Authors
                .Where(user => user.Id == 1)
                .FirstOrDefaultAsync(cancellationToken);

            var story = await context.Stories
                .AsNoTracking()
                .Where(entity => entity.Slug == request.Slug)
                .SingleAsync(cancellationToken);

            var comment = new Persistence.Models.Comment
            {
                Author = author,
                Content = request.Content,
                StoryId = story.Id,
                Created = dateTimeProvider.Now,
                IsPublic = request.IsPublic,
                Status = CommentStatus.Draft
            };

            await context.Comments.AddAsync(comment,cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return RequestResult.Success(mapper.Map<Comment>(comment));
        }
    }
}
