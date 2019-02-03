﻿using System.Linq;
using AutoMapper;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Persistence;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using Story = StoryBlog.Web.Services.Blog.Application.Stories.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, CommandResult<Story>>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly ISlugGenerator slugGenerator;
        private readonly IDateTimeProvider dateTimeProvider;

        public CreateStoryCommandHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ISlugGenerator slugGenerator,
            IDateTimeProvider dateTimeProvider)
        {
            this.context = context;
            this.mapper = mapper;
            this.slugGenerator = slugGenerator;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<CommandResult<Story>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
        {
            //var name = request.User.Identity.Name;
            var author = await context.Authors
                .Where(user => user.Id == 1)
                .FirstOrDefaultAsync(cancellationToken);
            var slug = slugGenerator.CreateFrom(request.Title);

            var story = new Persistence.Models.Story
            {
                Title = request.Title,
                Slug = slug,
                Content = request.Content,
                Status = StoryStatus.Draft,
                IsPublic = request.IsPublic,
                Created = dateTimeProvider.Now,
                Author = author
            };

            await context.Stories.AddAsync(story, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            var result = mapper.Map<Story>(story);

            return CommandResult.Success(result);
        }
    }
}