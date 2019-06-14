using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using System.Collections;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Shared.Infrastructure.Results
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class NavigateableQueryResult<TEntity> : RequestResult, INavigateableQueryResult<TEntity>
    {
        /// <inheritdoc cref="IQueryResult{TEntity}.Entities" />
        public IReadOnlyCollection<TEntity> Entities
        {
            get;
        }

        /// <inheritdoc cref="INavigateableQueryResult{TEntity}.Backward" />
        public NavigationCursor Backward
        {
            get;
        }

        /// <inheritdoc cref="INavigateableQueryResult{TEntity}.Forward" />
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
        public NavigateableQueryResult(
            IReadOnlyCollection<TEntity> entities,
            NavigationCursor backward = null,
            NavigationCursor forward = null)
            : base(true, false)
        {
            Entities = entities;
            Backward = backward;
            Forward = forward;
        }

        public IEnumerator<TEntity> GetEnumerator() => Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}