using MediatR;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using System.Security.Principal;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;

namespace StoryBlog.Web.Services.Blog.Application.Landing.Queries
{
    public sealed class GetLandingQuery : IRequest<IRequestResult<Models.Landing>>
    {
        public IPrincipal User
        {
            get;
        }

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

        public int StoriesFeedCount
        {
            get;
            set;
        }

        public GetLandingQuery(IPrincipal user)
        {
            User = user;
        }
    }
}
