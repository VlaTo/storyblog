using System;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListQuery : IRequest<IPagedQueryResult<Story>>
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
        public StoryIncludes Includes
        {
            get;
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
        public GetStoriesListQuery(IPrincipal user, StoryIncludes includes)
        {
            if (null == user)
            {
                throw new ArgumentNullException(nameof(user));
            }

            User = user;
            Includes = includes;
        }
    }
}