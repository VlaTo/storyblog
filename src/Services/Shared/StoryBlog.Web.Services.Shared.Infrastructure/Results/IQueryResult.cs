using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TResources"></typeparam>
    public interface IQueryResult<out TEntity, out TResources> : IRequestResult, IEnumerable<TEntity>
    where TResources : IQueryResultResources
    {
        /// <summary>
        /// 
        /// </summary>
        IReadOnlyCollection<TEntity> Data
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        TResources Resources
        {
            get;
        }
    }
}