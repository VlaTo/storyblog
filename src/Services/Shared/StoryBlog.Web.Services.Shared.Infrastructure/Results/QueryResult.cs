using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    public struct QueryResult<TEntity, TResources> : IQueryResult<TEntity, TResources>
        where TResources : IQueryResultResources
    {
        private IEnumerable<Exception> exceptions;
        private IReadOnlyCollection<TEntity> entities;

        /// <inheritdoc cref="IRequestResult.Exceptions" />
        public IEnumerable<Exception> Exceptions => exceptions ?? (exceptions = Enumerable.Empty<Exception>());

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<TEntity> Data => entities ?? (entities = new TEntity[0]);

        public TResources Resources
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public QueryResult(IReadOnlyCollection<TEntity> entities)
        {
            this.entities = entities;
            exceptions = null;
            Resources = default;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<TEntity> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}