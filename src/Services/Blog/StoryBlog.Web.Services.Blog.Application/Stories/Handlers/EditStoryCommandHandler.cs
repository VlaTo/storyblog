using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Services.Blog.Application.Extensions;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Commands;
using StoryBlog.Web.Services.Blog.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using Story = StoryBlog.Web.Services.Blog.Application.Stories.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EditStoryCommandHandler : IRequestHandler<EditStoryCommand, IRequestResult<Story>>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly ISlugGenerator slugGenerator;
        private readonly IDateTimeProvider dateTimeProvider;

        /// <summary>
        /// 
        /// </summary>
        public EditStoryCommandHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ISlugGenerator slugGenerator,
            IDateTimeProvider dateTimeProvider
        )
        {
            this.context = context;
            this.mapper = mapper;
            this.slugGenerator = slugGenerator;
            this.dateTimeProvider = dateTimeProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IRequestResult<Story>> Handle(EditStoryCommand request, CancellationToken cancellationToken)
        {
            var story = await context.Stories
                .AsTracking()
                .Include(entity => entity.Author)
                .SingleAsync(entity => entity.Slug == request.Slug, cancellationToken);

            /*var author = await context.Authors
                .Where(user => user.Id == 1)
                .FirstOrDefaultAsync(cancellationToken);*/

            var authorId = request.User.GetId();

            if (story.Author.Id != authorId)
            {
                return RequestResult.Error<Story>(new ArgumentException());
            }

            if (story.Title != request.Title)
            {
                story.Title = request.Title;
                story.Slug = slugGenerator.CreateFrom(request.Title);
            }

            story.Content = request.Content;
            story.Modified = dateTimeProvider.Now;

            await context.SaveChangesAsync(cancellationToken);

            var temp = mapper.Map<Story>(story);

            return RequestResult.Success(temp);
        }
    }
}