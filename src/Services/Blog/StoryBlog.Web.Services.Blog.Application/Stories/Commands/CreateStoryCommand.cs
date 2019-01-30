﻿using System.Security.Principal;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Commands
{
    public sealed class CreateStoryCommand : IRequest<CommandResult<Story>>
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

        public bool IsPublic
        {
            get;
        }

        public CreateStoryCommand(IPrincipal user, string title, string content, bool isPublic)
        {
            User = user;
            Title = title;
            Content = content;
            IsPublic = isPublic;
        }
    }
}