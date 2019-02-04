using System.Collections.Generic;
using System.Collections.ObjectModel;
using StoryBlog.Web.Services.Blog.Application.Models;

namespace StoryBlog.Web.Services.Blog.Application.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class PagedQueryResult<TEntity> : QueryResult<TEntity>
    {
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

        private PagedQueryResult(IReadOnlyCollection<TEntity> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static PagedQueryResult<TEntity> FromCollection(IEnumerable<TEntity> entities)
        {
            var list = new List<TEntity>(entities);
            var collection = new ReadOnlyCollection<TEntity>(list);
            return new PagedQueryResult<TEntity>(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static PagedQueryResult<TEntity> FromCollection(IList<TEntity> list)
        {
            var collection = new ReadOnlyCollection<TEntity>(list);
            return new PagedQueryResult<TEntity>(collection);
        }
    }
}