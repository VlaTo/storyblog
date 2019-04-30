using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using StoryBlog.Web.Services.Shared.Infrastructure.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Models
{
    /// <summary>
    /// 
    /// </summary>
    public struct PagedStoriesQueryResult : IPagedQueryResult<Story>
    {
        private IEnumerable<Exception> exceptions;
        private IReadOnlyCollection<Story> stories;
        private IReadOnlyCollection<Author> authors;

        /// <inheritdoc cref="IRequestResult.Exceptions" />
        public IEnumerable<Exception> Exceptions => exceptions ?? (exceptions = Enumerable.Empty<Exception>());

        /// <inheritdoc cref="IQueryResult{TEntity}.Entities" />
        public IReadOnlyCollection<Story> Entities => stories ?? (stories = new Story[0]);

        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<Author> Authors => authors ?? (authors = new Author[0]);

        /// <inheritdoc cref="IPagedQueryResult{TEntity}.Backward" />
        public NavigationCursor Backward
        {
            get;
            private set;
        }

        /// <inheritdoc cref="IPagedQueryResult{TEntity}.Forward" />
        public NavigationCursor Forward
        {
            get;
            private set;
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
        public static PagedStoriesQueryResult Create(
            IReadOnlyCollection<Story> stories,
            IReadOnlyCollection<Author> authors,
            NavigationCursor backward = null,
            NavigationCursor forward = null)
        {
            return new PagedStoriesQueryResult
            {
                stories = stories,
                authors = authors,
                Backward = backward,
                Forward = forward
            };
        }
    }
}