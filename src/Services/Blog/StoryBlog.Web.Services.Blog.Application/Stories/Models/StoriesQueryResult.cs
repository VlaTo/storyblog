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

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<Author> Authors
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

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<Story> GetEnumerator() => Entities.GetEnumerator();

        /// <inheritdoc cref="IEnumerable.GetEnumerator" />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stories"></param>
        /// <param name="authors"></param>
        /// <param name="backward"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
        public StoriesQueryResult(
            IReadOnlyCollection<Story> stories,
            IReadOnlyCollection<Author> authors = null,
            NavigationCursor backward = null,
            NavigationCursor forward = null)
            : base(true, false)
        {
            Entities = stories;
            Authors = authors;
            Backward = backward;
            Forward = forward;
        }
    }
}