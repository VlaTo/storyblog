using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Story = StoryBlog.Web.Services.Blog.Application.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class CreateStoryCommandHandler : IRequestHandler<CreateStoryCommand, IRequestResult<Story>>
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

        public async Task<IRequestResult<Story>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
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

            return RequestResult.Success(mapper.Map<Story>(story));
        }
    }
}