using System.Threading;
using System.Threading.Tasks;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Comments.Commands;
using StoryBlog.Web.Services.Blog.Application.Infrastructure;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, IRequestResult>
    {
        /// <summary>
        /// 
        /// </summary>
        public DeleteCommentCommandHandler()
        {
        }

        /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle" />
        public Task<IRequestResult> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}