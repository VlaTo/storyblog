using MediatR;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DeleteStoryCommandHandler : IRequestHandler<DeleteStoryCommand, IRequestResult>
    {
        private readonly StoryBlogDbContext context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public DeleteStoryCommandHandler(StoryBlogDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IRequestResult> Handle(DeleteStoryCommand request, CancellationToken cancellationToken)
        {
            if (null == request)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var story = await context.Stories.SingleAsync(entity => entity.Slug == request.Slug, cancellationToken);

            using (var transaction = await context.Database.BeginTransactionAsync(cancellationToken))
            {
                context.Stories.Remove(story);

                await context.SaveChangesAsync(cancellationToken);

                transaction.Commit();
            }

            return new RequestResult();
        }
    }
}