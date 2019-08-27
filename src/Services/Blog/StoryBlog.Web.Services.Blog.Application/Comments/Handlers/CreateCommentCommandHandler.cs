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

            if (false == story.IsCommentsClosed)
            {
                return CreateCommentResult.Failed(CreateCommentFailedReason.CommentsClosed);
            }

            var parent = request.ParentId.HasValue
                ? context.Comments.Find(request.ParentId.Value)
                : null;
            var comment = new Persistence.Models.Comment
            {
                Author = author,
                Content = request.Content,
                StoryId = story.Id,
                Parent = parent,
                Created = dateTimeProvider.UtcNow,
                IsPublic = request.IsPublic,
                Status = identity.IsAuthenticated ? CommentStatus.Published : CommentStatus.Draft
            };

            await context.Comments.AddAsync(comment,cancellationToken);

            if (null != parent)
            {
                parent.Comments.Add(comment);
            }

            await context.SaveChangesAsync(cancellationToken);

            return CreateCommentResult.Success(mapper.Map<Comment>(comment));
        }
    }
}
