using MediatR;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using System.Security.Principal;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Commands
{
    public sealed class DeleteStoryCommand : IRequest<IRequestResult>
    {
        public IPrincipal User
        {
            get;
        }

        public string Slug
        {
            get;
        }

        public DeleteStoryCommand(IPrincipal user, string slug)
        {
            User = user;
            Slug = slug;
        }
    }
}