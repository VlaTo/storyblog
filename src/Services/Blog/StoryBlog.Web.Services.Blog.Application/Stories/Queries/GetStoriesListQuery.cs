using MediatR;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using System;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListQuery : IRequest<PagedStoriesQueryResult>
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
        public bool IncludeAuthors
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IncludeComments
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public NavigationCursor Cursor
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="includes"></param>
        public GetStoriesListQuery(IPrincipal user)
        {
            if (null == user)
            {
                throw new ArgumentNullException(nameof(user));
            }

            User = user;
        }
    }
}