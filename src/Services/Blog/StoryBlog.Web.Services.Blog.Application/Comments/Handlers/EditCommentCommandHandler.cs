using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Comments.Commands;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EditCommentCommandHandler : IRequestHandler<EditCommentCommand, IRequestResult<Comment>>
    {
        /// <summary>
        /// 
        /// </summary>
        public EditCommentCommandHandler()
        {
        }

        /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle" />
        public Task<IRequestResult<Comment>> Handle(EditCommentCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}