using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    public sealed class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, StoryModel>
    {
        private readonly StoryBlogDbContext context;
        private readonly IDateTimeProvider dateTimeProvider;

        public CreateStoryCommandHandler(
            StoryBlogDbContext context,
            IDateTimeProvider dateTimeProvider)
        {
            this.context = context;
            this.dateTimeProvider = dateTimeProvider;
        }

        public Task<StoryModel> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
        {
            var name = request.User.Identity.Name;

            using (var transaction = context.Database.BeginTransaction())
            {
                var story = new Story
                {
                    Title = request.Title,
                    Content = request.Content
                };

                context.Stories.Add(story);

                transaction.Commit();
            }

            return Task.FromResult(new StoryModel
            {

            });
        }
    }
}
