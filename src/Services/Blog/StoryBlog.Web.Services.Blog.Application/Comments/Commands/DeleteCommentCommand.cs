using System.Security.Principal;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DeleteCommentCommand : IRequest<IRequestResult>
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

        public DeleteCommentCommand(IPrincipal user, long id)
        {
            User = user;
            Id = id;
        }
    }
}