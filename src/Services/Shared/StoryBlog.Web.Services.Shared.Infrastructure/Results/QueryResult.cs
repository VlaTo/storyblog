using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    public struct QueryResult<TEntity> : IQueryResult<TEntity>
    {
        private IEnumerable<Exception> exceptions;
        private IReadOnlyCollection<TEntity> entities;

        /// <inheritdoc cref="IRequestResult.Exceptions" />
        public IEnumerable<Exception> Exceptions => exceptions ?? (exceptions = Enumerable.Empty<Exception>());

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<TEntity> Entities => entities ?? (entities = new TEntity[0]);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public QueryResult(IReadOnlyCollection<TEntity> entities)
        {
            this.entities = entities;
            exceptions = null;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<TEntity> GetEnumerator()=> Entities.GetEnumerator();

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()=> GetEnumerator();
    }
}