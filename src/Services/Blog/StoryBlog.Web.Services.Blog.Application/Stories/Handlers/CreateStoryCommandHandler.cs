using AutoMapper;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Persistence;
using System.Threading;
using System.Threading.Tasks;
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

        public Task<CommandResult<Story>> Handle(CreateStoryCommand request, CancellationToken cancellationToken)
        {
            var name = request.User.Identity.Name;
            Story result;

            using (var transaction = context.Database.BeginTransaction())
            {
                var story = new Persistence.Models.Story
                {
                    Title = request.Title,
                    Slug = slugGenerator.CreateFrom(request.Title),
                    Content = request.Content,
                    Created = dateTimeProvider.Now
                };

                context.Stories.Add(story);

                transaction.Commit();

                result = mapper.Map<Story>(story);
            }

            return Task.FromResult(CommandResult.Ok(result));
        }
    }
}
