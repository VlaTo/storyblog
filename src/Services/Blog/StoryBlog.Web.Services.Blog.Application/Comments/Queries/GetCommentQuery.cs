﻿using MediatR;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetCommentQuery : IRequest<IRequestResult<Comment>>
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
        public long Id
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IncludeAuthor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="id"></param>
        public GetCommentQuery(IPrincipal user, long id)
        {
            User = user;
            Id = id;
        }
    }
}