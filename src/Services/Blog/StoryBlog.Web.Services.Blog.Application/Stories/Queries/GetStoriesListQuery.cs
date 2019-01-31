using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoriesListQuery : IRequest<IReadOnlyCollection<Story>>
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
        public GetStoriesListQuery(
            IPrincipal user)
        {
            User = user;
        }
    }
}