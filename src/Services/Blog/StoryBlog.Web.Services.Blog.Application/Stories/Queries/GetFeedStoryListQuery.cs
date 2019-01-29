using System.Collections.Generic;
using System.Security.Principal;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetFeedStoryListQuery : IRequest<IReadOnlyCollection<StoryModel>>
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
        public GetFeedStoryListQuery(IPrincipal user)
        {
            User = user;
        }
    }
}