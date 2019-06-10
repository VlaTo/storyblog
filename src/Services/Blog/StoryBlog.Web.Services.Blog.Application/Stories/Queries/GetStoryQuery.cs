using MediatR;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetStoryQuery : IRequest<StoryRequestResult>
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
        /// <param name="user"></param>
        /// <param name="slug"></param>
        public GetStoryQuery(IPrincipal user, string slug)
        {
            User = user;
            Slug = slug;
        }
    }
}