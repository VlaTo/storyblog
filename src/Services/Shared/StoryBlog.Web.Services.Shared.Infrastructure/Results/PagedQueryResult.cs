using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public struct PagedQueryResult<TEntity> : IPagedQueryResult<TEntity>
    {
        private IEnumerable<Exception> exceptions;
        private IReadOnlyCollection<TEntity> entities;

        /// <inheritdoc cref="IRequestResult.Exceptions" />
        public IEnumerable<Exception> Exceptions => exceptions ?? (exceptions = Enumerable.Empty<Exception>());

        /// <inheritdoc cref="IQueryResult{TEntity}.Entities" />
        public IReadOnlyCollection<TEntity> Entities => entities ?? (entities = new TEntity[0]);

        /// <inheritdoc cref="IPagedQueryResult{TEntity}.Backward" />
        public NavigationCursor Backward
        {
            get;
        }

        /// <inheritdoc cref="IPagedQueryResult{TEntity}.Forward" />
        public NavigationCursor Forward
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="backward"></param>
        /// <param name="forward"></param>
        public PagedQueryResult(
            IReadOnlyCollection<TEntity> entities,
            NavigationCursor backward = null,
            NavigationCursor forward = null)
        {
            this.entities = entities;

            exceptions = null;

            Backward = backward;
            Forward = forward;
        }

        public IEnumerator<TEntity> GetEnumerator() => Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="backward"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static IPagedQueryResult<TEntity> Success(
            IEnumerable<TEntity> entities,
            NavigationCursor backward = null,
            NavigationCursor forward = null)
        {
            var list = new List<TEntity>(entities);
            var collection = new ReadOnlyCollection<TEntity>(list);
            return new PagedQueryResult<TEntity>(collection, backward, forward);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IPagedQueryResult<TEntity> Success(IList<TEntity> entities)
        {
            var collection = new ReadOnlyCollection<TEntity>(entities);
            return new PagedQueryResult<TEntity>(collection);
        }
    }
}