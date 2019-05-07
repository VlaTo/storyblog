using MediatR;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateCommentCommand : IRequest<IRequestResult<Comment>>
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
        /// <param name="slug"></param>
        /// <param name="content"></param>
        /// <param name="isPublic"></param>
        public CreateCommentCommand(IPrincipal user, string slug, string content, bool isPublic)
        {
            User = user;
            Slug = slug;
            Content = content;
            IsPublic = isPublic;
        }
    }
}
