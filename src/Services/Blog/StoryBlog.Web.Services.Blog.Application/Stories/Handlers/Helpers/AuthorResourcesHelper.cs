using AutoMapper;
using StoryBlog.Web.Services.Blog.Application.Models;
using System;
using System.Collections.Generic;

namespace StoryBlog.Web.Services.Blog.Application.Stories.Handlers.Helpers
{
    internal static class AuthorResourcesHelper
    {
        internal static void CreateMappedStories(
            IMapper mapper,
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

                CreateMappedCommentsForStory(mapper, model.Comments, story.Comments, getMappedAuthor, includeAuthors);

                stories.Add(model);
            }
        }

        internal static void CreateMappedCommentsForStory(
            IMapper mapper,
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
