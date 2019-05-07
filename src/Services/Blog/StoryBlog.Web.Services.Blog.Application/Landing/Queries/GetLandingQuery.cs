using MediatR;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;

namespace StoryBlog.Web.Services.Blog.Application.Landing.Queries
{
    public sealed class GetLandingQuery : IRequest<IRequestResult<Models.Landing>>
    {
        public bool IncludeHeroStory
        {
            get;
            set;
        }

        public int FeaturedStoriesCount
        {
            get;
            set;
        }
    }
}
