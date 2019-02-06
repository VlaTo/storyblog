using System;
using System.Collections.Generic;
using System.Text;
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
    public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, IRequestResult<Comment>>
    {
        /// <summary>
        /// 
        /// </summary>
        public CreateCommentCommandHandler()
        {
        }

        /// <inheritdoc cref="IRequestHandler{TRequest,TResponse}.Handle" />
        public Task<IRequestResult<Comment>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
