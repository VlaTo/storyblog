using StoryBlog.Web.Services.Blog.Application.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
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

        /// <summary>
        /// 
        /// </summary>
        public NavigationCursor Backward
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public NavigationCursor Forward
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        public PagedQueryResult(IReadOnlyCollection<TEntity> entities)
        {
            this.entities = entities;
            exceptions = null;

            Backward = null;
            Forward = null;
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return Entities.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

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
            return new PagedQueryResult<TEntity>(collection)
            {
                Backward = backward,
                Forward = forward
            };
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