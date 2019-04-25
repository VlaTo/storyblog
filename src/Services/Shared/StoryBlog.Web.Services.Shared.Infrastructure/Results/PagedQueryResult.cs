using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public struct PagedQueryResult<TEntity, TResources> : IPagedQueryResult<TEntity, TResources>
        where TResources : IQueryResultResources
    {
        private IEnumerable<Exception> exceptions;
        private IReadOnlyCollection<TEntity> entities;

        /// <inheritdoc cref="IRequestResult.Exceptions" />
        public IEnumerable<Exception> Exceptions => exceptions ?? (exceptions = Enumerable.Empty<Exception>());

        /// <inheritdoc cref="IQueryResult{TEntity}.Data" />
        public IReadOnlyCollection<TEntity> Data => entities ?? (entities = new TEntity[0]);

        /// <summary>
        /// 
        /// </summary>
        public TResources Resources
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public PagedQueryResult(IReadOnlyCollection<TEntity> entities)
        {
            this.entities = entities;
            exceptions = null;

            Resources = default;
            Backward = null;
            Forward = null;
        }

        public IEnumerator<TEntity> GetEnumerator() => Data.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="backward"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public static IPagedQueryResult<TEntity, TResources> Success(
            IEnumerable<TEntity> entities,
            NavigationCursor backward = null,
            NavigationCursor forward = null)
        {
            var list = new List<TEntity>(entities);
            var collection = new ReadOnlyCollection<TEntity>(list);
            return new PagedQueryResult<TEntity, TResources>(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IPagedQueryResult<TEntity, TResources> Success(IList<TEntity> entities)
        {
            var collection = new ReadOnlyCollection<TEntity>(entities);
            return new PagedQueryResult<TEntity, TResources>(collection);
        }

        public NavigationCursor Backward { get; }
        public NavigationCursor Forward { get; }
    }
}