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
    public sealed class PagedStoriesQueryResources : IQueryResultResources
    {
        /// <summary>
        /// 
        /// </summary>
        public IReadOnlyCollection<Author> Authors
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public struct PagedStoriesQueryResult : IPagedQueryResult<Story, PagedStoriesQueryResources>
    {
        private IEnumerable<Exception> exceptions;
        private IReadOnlyCollection<Story> stories;

        /// <inheritdoc cref="IRequestResult.Exceptions" />
        public IEnumerable<Exception> Exceptions => exceptions ?? (exceptions = Enumerable.Empty<Exception>());

        /// <inheritdoc cref="IQueryResult{TEntity,TResources}.Data" />
        public IReadOnlyCollection<Story> Data => stories ?? (stories = new Story[0]);

        /// <inheritdoc cref="IQueryResult{TEntity,TResources}.Resources" />
        public PagedStoriesQueryResources Resources
        {
            get;
            private set;
        }

        /// <inheritdoc cref="IPagedQueryResult{TEntity,TResources}.Backward" />
        public NavigationCursor Backward
        {
            get;
            private set;
        }

        /// <inheritdoc cref="IPagedQueryResult{TEntity,TResources}.Forward" />
        public NavigationCursor Forward
        {
            get;
            private set;
        }

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator" />
        public IEnumerator<Story> GetEnumerator() => Data.GetEnumerator();

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
                Resources = new PagedStoriesQueryResources
                {
                    Authors = authors
                },
                Backward = backward,
                Forward = forward
            };
        }
    }
}