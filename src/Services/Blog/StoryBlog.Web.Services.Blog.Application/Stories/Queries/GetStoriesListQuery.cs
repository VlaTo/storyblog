using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
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
        /// <param name="user"></param>
        /// <param name="includes"></param>
        public GetStoriesListQuery(IPrincipal user, int pageSize, params string[] includes)
        {
            User = user;
        }
    }
}