﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Application.Stories.Models;
using StoryBlog.Web.Services.Blog.Application.Stories.Queries;
using StoryBlog.Web.Services.Blog.Persistence;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using StoryBlog.Web.Services.Shared.Infrastructure.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Author = StoryBlog.Web.Services.Blog.Application.Models.Author;
using Comment = StoryBlog.Web.Services.Blog.Application.Models.Comment;
using Story = StoryBlog.Web.Services.Blog.Application.Models.Story;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public sealed class GetStoriesListQueryHandler : IRequestHandler<GetStoriesListQuery, PagedStoriesQueryResult>
    {
        private readonly StoryBlogDbContext context;
        private readonly IMapper mapper;
        private readonly ILogger<GetStoriesListQuery> logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public GetStoriesListQueryHandler(
            StoryBlogDbContext context,
            IMapper mapper,
            ILogger<GetStoriesListQuery> logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<PagedStoriesQueryResult> Handle(GetStoriesListQuery request, CancellationToken cancellationToken)
        {
            if (null == request)
            {
                throw new ArgumentNullException(nameof(request));
            }

            logger.LogDebug($"Executing request: {request.GetType().Name}");

            var queryable = context.Stories.AsNoTracking();

            if (request.User.Identity.IsAuthenticated)
            {
                var authorId = 0;
                queryable = queryable.Where(story => story.Status == StoryStatus.Published || story.AuthorId == authorId);
            }
            else
            {
                queryable = queryable.Where(story => story.Status == StoryStatus.Published && story.IsPublic);
            }

            queryable = queryable.OrderBy(story => story.Id);

            var id = queryable.Select(story => story.Id).FirstOrDefault();

            switch (request.Cursor.Direction)
            {
                case NavigationCursorDirection.Backward:
                {
                    //stories = stories.SkipWhile(story => story.Id < request.Cursor.Id);
                    queryable = queryable.Where(story => story.Id < request.Cursor.Id);
                    break;
                }

                case NavigationCursorDirection.Forward:
                {
                    queryable = queryable.Where(story => story.Id > request.Cursor.Id);
                    break;
                }
            }

            queryable = queryable.Take(request.Cursor.Count);

            if (request.IncludeAuthors)
            {
                queryable = queryable.Include(story => story.Author);
            }

            if (request.IncludeComments)
            {
                if (request.IncludeAuthors)
                {
                    queryable = queryable.Include(story => story.Comments).ThenInclude(comment => comment.Author);
                }
                else
                {
                    queryable = queryable.Include(story => story.Comments);
                }
            }

            var entities = await queryable.Take(request.Cursor.Count).ToListAsync(cancellationToken);
            var stories = new Collection<Story>();
            var authors = new Collection<Author>();

            CreateMappedStories(stories, authors, entities, request.IncludeAuthors);

            return PagedStoriesQueryResult.Create(
                stories,
                authors,
                backward: GetBackwardCursor(id, stories, request.Cursor.Count),
                forward: GetForwardCursor(stories, request.Cursor.Count)
            );
        }

        private static NavigationCursor GetBackwardCursor(long? firstId, IList<Story> stories, int pageSize)
        {
            if (0 == stories.Count)
            {
                return null;
            }

            var id = stories[0].Id;

            if (firstId >= id)
            {
                return null;
            }

            return NavigationCursor.Backward(id - pageSize, pageSize);
        }

        private static NavigationCursor GetForwardCursor(IList<Story> stories, int pageSize)
        {
            if (0 == stories.Count)
            {
                return null;
            }

            if (pageSize > stories.Count)
            {
                return null;
            }

            var id = stories[stories.Count - 1].Id;

            return NavigationCursor.Forward(id, pageSize);
        }

        private void CreateMappedStories(
            ICollection<Story> stories,
            ICollection<Author> authors,
            IEnumerable<Persistence.Models.Story> source,
            bool includeAuthors)
        {
            var cache = new Dictionary<long, Author>();
            var getMappedAuthor = new Func<Persistence.Models.Author, Author>(author =>
            {
                if (cache.TryGetValue(author.Id, out var entity))
                {
                    return entity;
                }

                entity = mapper.Map<Author>(author);

                authors.Add(entity);
                cache[author.Id] = entity;

                return entity;
            });

            foreach (var author in authors)
            {
                cache[author.Id] = author;
            }

            foreach (var story in source)
            {
                var model = mapper.Map<Story>(story);

                if (includeAuthors && null != story.Author)
                {
                    model.Author = getMappedAuthor.Invoke(story.Author);
                }

                CreateMappedCommentsForStory(model.Comments, story.Comments, getMappedAuthor, includeAuthors);

                stories.Add(model);
            }
        }

        private void CreateMappedCommentsForStory(
            ICollection<Comment> comments,
            IEnumerable<Persistence.Models.Comment> source,
            Func<Persistence.Models.Author, Author> getMappedAuthor,
            bool includeAuthors)
        {
            foreach (var entity in source)
            {
                var comment = mapper.Map<Comment>(entity);

                if (includeAuthors && null != entity.Author)
                {
                    comment.Author = getMappedAuthor.Invoke(entity.Author);
                }

                comments.Add(comment);
            }
        }
    }
}