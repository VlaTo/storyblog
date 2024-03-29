﻿using MediatR;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EditStoryCommand : IRequest<IRequestResult<Story>>
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
        public string Slug
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
        public EditStoryCommand(IPrincipal user, string slug, string title, string content, bool isPublic)
        {
            User = user;
            Slug = slug;
            Title = title;
            Content = content;
            IsPublic = isPublic;
        }
    }
}