using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IQueryResult<out TEntity> : IRequestResult, IEnumerable<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        IReadOnlyCollection<TEntity> Entities
        {
            get;
        }
    }
}