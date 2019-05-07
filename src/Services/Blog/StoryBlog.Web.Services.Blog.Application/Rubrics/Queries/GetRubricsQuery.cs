using System.Collections.Generic;
using MediatR;
using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;

namespace StoryBlog.Web.Services.Blog.Application.Rubrics.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GetRubricsQuery : IRequest<IRequestResult<IReadOnlyCollection<Rubric>>>
    {
    }
}