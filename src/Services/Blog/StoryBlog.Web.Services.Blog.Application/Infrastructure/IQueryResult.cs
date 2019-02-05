using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    public interface IQueryResult<out TEntity> : IRequestResult, IEnumerable<TEntity>
    {
        IReadOnlyCollection<TEntity> Entities { get; }
    }
}