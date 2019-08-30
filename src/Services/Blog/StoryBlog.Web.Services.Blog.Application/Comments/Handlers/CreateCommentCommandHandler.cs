using System;
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
using Microsoft.Extensions.Logging;
using Comment = StoryBlog.Web.Services.Blog.Application.Models.Comment;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CreateCommentResult>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;
        private readonly ILogger<CreateCommentCommandHandler> logger;

        /// <summary>
        /// 
        /// </summary>
        public CreateCommentCommandHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider,
            ILogger<CreateCommentCommandHandler> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
            this.logger = logger;
        }

        /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle" />
        public async Task<CreateCommentResult> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var identity = request.User.Identity;
            var author = await context.Authors
                .AsNoTracking()
                .Where(user => user.Id == 1)
                .FirstOrDefaultAsync(cancellationToken);

            var story = await context.Stories
                .AsNoTracking()
                .Where(entity => entity.Slug == request.Slug)
                .SingleAsync(cancellationToken);

            if (null == story)
            {
                return CreateCommentResult.Failed(CreateCommentFailedReason.NoStoryFound);
            }

            if (StoryStatus.Published != story.Status)
            {
                return CreateCommentResult.Failed(CreateCommentFailedReason.StoryNotPublished);
            }

            if (story.IsCommentsClosed)
            {
                return CreateCommentResult.Failed(CreateCommentFailedReason.CommentsClosed);
            }

            if (request.ParentId.HasValue)
            {
                var id = request.ParentId.Value;
                var count = await context.Comments.CountAsync(entry => entry.Id == id, cancellationToken);

                if (count == 0)
                {
                    return CreateCommentResult.Failed(CreateCommentFailedReason.SpecifiedParentNotFound);
                }
            }

            try
            {
                var comment = new Persistence.Models.Comment
                {
                    Content = request.Content,
                    StoryId = story.Id,
                    ParentId = request.ParentId,
                    AuthorId = author.Id,
                    Created = dateTimeProvider.UtcNow,
                    IsPublic = request.IsPublic,
                    Status = identity.IsAuthenticated ? CommentStatus.Published : CommentStatus.Draft
                };

                await context.Comments.AddAsync(comment, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                var commentId = comment.Id;

                //comment = await context.Comments.FindAsync(new object[] {comment.Id}, cancellationToken: cancellationToken);
                comment = await context.Comments
                    .AsNoTracking()
                    .Include(entry => entry.Author)
                    .Include(entry => entry.Parent)
                    .Where(entry => entry.Id == commentId)
                    .SingleAsync(cancellationToken);

                return CreateCommentResult.Success(mapper.Map<Comment>(comment));
            }
            catch (Exception exception)
            {
                logger.LogError(exception, "Create comment failed");
                return CreateCommentResult.Failed(CreateCommentFailedReason.Unknown);
            }
        }
    }
}
