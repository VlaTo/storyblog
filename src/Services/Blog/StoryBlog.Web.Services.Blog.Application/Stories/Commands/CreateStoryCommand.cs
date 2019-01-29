using System.Security.Principal;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Commands
{
    public sealed class CreateStoryCommand : IRequest<StoryModel>
    {
        public IPrincipal User
        {
            get;
        }

        public string Title
        {
            get;
        }

        public string Content
        {
            get;
        }

        public CreateStoryCommand(IPrincipal user, string title, string content)
        {
            User = user;
            Title = title;
            Content = content;
        }
    }
}