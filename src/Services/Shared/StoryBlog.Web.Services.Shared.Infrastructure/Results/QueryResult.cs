using System.Collections;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryResult<TEntity> : RequestResult, IQueryResult<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<TEntity> Entities
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public QueryResult(IReadOnlyCollection<TEntity> entities)
            : base(true, false)
        {
            Entities = entities;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<TEntity> GetEnumerator()=> Entities.GetEnumerator();

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()=> GetEnumerator();
    }
}