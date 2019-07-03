using StoryBlog.Web.Services.Blog.Application.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System.Collections;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoriesQueryResult : RequestResult, INavigateableQueryResult<Story>
    {
        /// <inheritdoc cref="IQueryResult{TEntity}.Entities" />
        public IReadOnlyCollection<Story> Entities
        {
            get;
        }

        /// <inheritdoc cref="INavigateableQueryResult{TEntity}.Backward" />
        public NavigationCursor Backward
        {
            get;
            private set;
        }

        /// <inheritdoc cref="INavigateableQueryResult{TEntity}.Forward" />
        public NavigationCursor Forward
        {
            get;
            private set;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<Story> GetEnumerator() => Entities.GetEnumerator();

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private StoriesQueryResult(IReadOnlyCollection<Story> stories)
            : base(true, false)
        {
            Entities = stories;
        }

        public static StoriesQueryResult Success(
            IReadOnlyCollection<Story> stories,
            NavigationCursor backward = null,
            NavigationCursor forward = null)
        {
            return new StoriesQueryResult(stories)
            {
                Backward = backward,
                Forward = forward
            };
        }
    }
}