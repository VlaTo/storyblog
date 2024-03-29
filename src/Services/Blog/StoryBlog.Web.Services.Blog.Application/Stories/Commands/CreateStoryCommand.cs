﻿using MediatR;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateStoryCommand : IRequest<IRequestResult<Story>>
    {
        /// <summary>
        /// 
        /// </summary>
        public IPrincipal User
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Content
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsPublic
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="isPublic"></param>
        public CreateStoryCommand(IPrincipal user, string title, string content, bool isPublic)
        {
            User = user;
            Title = title;
            Content = content;
            IsPublic = isPublic;
        }
    }
}