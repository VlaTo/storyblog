using System.Security.Principal;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.Application.Landing.Queries
{
    public sealed class GetLandingQuery : IRequest<IRequestResult<Models.Landing>>
    {
        public IPrincipal User
        {
            get;
        }

        public bool IncludeHeroPost
        {
            get;
            set;
        }

        public int FeaturedPostsCount
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
