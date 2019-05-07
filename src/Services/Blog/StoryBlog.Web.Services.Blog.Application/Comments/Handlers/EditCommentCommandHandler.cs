using MediatR;
using StoryBlog.Web.Services.Blog.Application.Comments.Commands;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Threading;
using System.Threading.Tasks;

namespace StoryBlog.Web.Services.Blog.Application.Comments.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
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