using MediatR;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Security.Principal;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EditCommentCommand : IRequest<IRequestResult<Comment>>
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
        /// <param name="id"></param>
        /// <param name="content"></param>
        /// <param name="isPublic"></param>
        public EditCommentCommand(IPrincipal user, long id, string content, bool isPublic)
        {
            User = user;
            Id = id;
            Content = content;
            IsPublic = isPublic;
        }
    }
}