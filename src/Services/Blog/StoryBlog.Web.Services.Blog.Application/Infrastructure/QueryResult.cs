using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryResult<TEntity> : RequestResult, IEnumerable<TEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<TEntity> Collection
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public QueryResult(IReadOnlyCollection<TEntity> collection)
            : base(Enumerable.Empty<Exception>())
        {
            Collection = collection;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<TEntity> GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}