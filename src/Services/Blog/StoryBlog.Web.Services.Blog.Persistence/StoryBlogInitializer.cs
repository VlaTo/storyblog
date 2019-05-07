using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StoryBlog.Web.Services.Blog.Persistence.Models;
using StoryBlog.Web.Services.Shared.Data.Csv;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace StoryBlog.Web.Services.Blog.Persistence
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryBlogInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        public static void Seed(StoryBlogDbContext context, Assembly resourceAssembly, ILogger logger)
        {
            SeedSettings(context, resourceAssembly, logger);
            SeedAuthors(context, resourceAssembly, logger);
            SeedStories(context, resourceAssembly, logger);
            SeedComments(context, resourceAssembly, logger);
            SeedRubrics(context, resourceAssembly, logger);
        }

        private static void SeedAuthors(StoryBlogDbContext context, Assembly resourceAssembly, ILogger logger)
        {
            var resource = resourceAssembly.GetManifestResourceStream("StoryBlog.Web.Services.Blog.API.Data.Authors.csv");

            using (var reader = new StreamReader(resource, Encoding.UTF8))
            {
                var document = CsvDocument.CreateFrom(reader);

                using (var transaction = context.Database.BeginTransaction())
                {
                    foreach (var row in document.Rows)
                    {
                        var author = CreateAuthorFromRow(context, row);
                        logger.LogDebug($"[SeedAuthors] Author with name \'{author.UserName}\' created");
                    }

                    transaction.Commit();
                }
            }
        }

        private static void SeedSettings(StoryBlogDbContext context, Assembly resourceAssembly, ILogger logger)
        {
            var resource = resourceAssembly.GetManifestResourceStream("StoryBlog.Web.Services.Blog.API.Data.Settings.csv");

            using (var reader = new StreamReader(resource, Encoding.UTF8))
            {
                var document = CsvDocument.CreateFrom(reader);

                using (var transaction = context.Database.BeginTransaction())
                {
                    foreach (var row in document.Rows)
                    {
                        var settings = CreateSettingFromRow(context, row);
                        logger.LogDebug($"[SeedSettings] Setting \'{settings.Name}\' created");
                    }

                    transaction.Commit();
                }
            }
        }

        private static void SeedStories(StoryBlogDbContext context, Assembly resourceAssembly, ILogger logger)
        {
            var resource = resourceAssembly.GetManifestResourceStream("StoryBlog.Web.Services.Blog.API.Data.Stories.csv");

            using (var reader = new StreamReader(resource, Encoding.UTF8))
            {
                var document = CsvDocument.CreateFrom(reader);

                using (var transaction = context.Database.BeginTransaction())
                {
                    foreach (var row in document.Rows)
                    {
                        var story = CreateStoryFromRow(context, row);
                        logger.LogDebug($"[SeedStories] Story \'{story.Slug}\' created");
                    }

                    transaction.Commit();
                }
            }
        }

        private static void SeedComments(StoryBlogDbContext context, Assembly resourceAssembly, ILogger logger)
        {
            var resource = resourceAssembly.GetManifestResourceStream("StoryBlog.Web.Services.Blog.API.Data.Comments.csv");

            using (var reader = new StreamReader(resource, Encoding.UTF8))
            {
                var document = CsvDocument.CreateFrom(reader);

                using (var transaction = context.Database.BeginTransaction())
                {
                    foreach (var row in document.Rows)
                    {
                        var comment = CreateCommentFromRow(context, row);
                        logger.LogDebug($"[SeedComment] Comment \'{comment.Id}\' created");
                    }

                    transaction.Commit();
                }
            }
        }

        private static void SeedRubrics(StoryBlogDbContext context, Assembly resourceAssembly, ILogger logger)
        {
            var resource = resourceAssembly.GetManifestResourceStream("StoryBlog.Web.Services.Blog.API.Data.Rubrics.csv");

            using (var reader = new StreamReader(resource, Encoding.UTF8))
            {
                var document = CsvDocument.CreateFrom(reader);

                using (var transaction = context.Database.BeginTransaction())
                {
                    foreach (var row in document.Rows)
                    {
                        var rubric = CreateRubricFromRow(context, row);
                        logger.LogDebug($"[SeedRubric] Rubric \'{rubric.Id}\' created");
                    }

                    transaction.Commit();
                }
            }
        }

        private static Author CreateAuthorFromRow(StoryBlogDbContext context, CsvRow row)
        {
            var userName = row.Fields[1].Text;
            var author = context.Authors
                .AsNoTracking()
                .SingleOrDefault(entity => entity.UserName == userName);

            if (null != author)
            {
                return author;
            }

            author = new Author
            {
                Id = row.Fields[0].ReadAs<long>(),
                UserName = userName
            };

            context.Authors.Add(author);
            context.SaveChanges();

            return author;
        }

        private static Settings CreateSettingFromRow(StoryBlogDbContext context, CsvRow row)
        {
            var name = row.Fields[0].Text;
            var entity = context.Settings
                .AsNoTracking()
                .SingleOrDefault(setting => setting.Name == name);

            if (null != entity)
            {
                return entity;
            }

            entity = new Settings
            {
                Name = name,
                Value = Encoding.UTF8.GetBytes(row.Fields[1].Text)
            };

            context.Settings.Add(entity);
            context.SaveChanges();

            return entity;
        }

        private static Story CreateStoryFromRow(StoryBlogDbContext context, CsvRow row)
        {
            var slug = row.Fields[2].Text;
            var entity = context.Stories
                .AsNoTracking()
                .SingleOrDefault(story => story.Slug == slug);

            if (null != entity)
            {
                return entity;
            }

            entity = new Story
            {
                Id = row.Fields[0].ReadAs<uint>(),
                Title = row.Fields[1].Text,
                Slug = slug,
                Content = row.Fields[3].Text,
                AuthorId = row.Fields[4].ReadAs<long>(),
                Status = (StoryStatus) Enum.ToObject(typeof(StoryStatus), row.Fields[5].ReadAs<int>()),
                Created = row.Fields[6].ReadAs<DateTime>(),
                Modified = null == row.Fields[7].Text ? null : new DateTime?(row.Fields[7].ReadAs<DateTime>()),
                IsPublic = row.Fields[8].ReadAs<bool>()
            };

            context.Stories.Add(entity);
            context.SaveChanges();

            return entity;
        }

        private static Comment CreateCommentFromRow(StoryBlogDbContext context, CsvRow row)
        {
            var id = row.Fields[0].ReadAs<long>();
            var entity = context.Comments
                .AsNoTracking()
                .SingleOrDefault(comment => comment.Id == id);

            if (null != entity)
            {
                return entity;
            }

            entity = new Comment
            {
                Id = id,
                Content = row.Fields[1].Text,
                AuthorId = row.Fields[2].ReadAs<long>(),
                StoryId = row.Fields[3].ReadAs<long>(),
                ParentId = null == row.Fields[4].Text ? null : new long?(row.Fields[4].ReadAs<long>()),
                IsPublic = row.Fields[5].ReadAs<bool>(),
                Status = (CommentStatus) Enum.ToObject(typeof(CommentStatus), row.Fields[6].ReadAs<int>()),
                Created = row.Fields[7].ReadAs<DateTime>(),
                Modified = null == row.Fields[8].Text ? null : new DateTime?(row.Fields[8].ReadAs<DateTime>())
            };

            context.Comments.Add(entity);
            context.SaveChanges();

            return entity;
        }

        private static Rubric CreateRubricFromRow(StoryBlogDbContext context, CsvRow row)
        {
            var id = row.Fields[0].ReadAs<long>();
            var entity = context.Rubrics
                .AsNoTracking()
                .SingleOrDefault(rubric => rubric.Id == id);

            if (null != entity)
            {
                return entity;
            }

            entity = new Rubric
            {
                Id = id,
                Name = row.Fields[1].Text,
                Slug = row.Fields[2].Text
            };

            context.Rubrics.Add(entity);
            context.SaveChanges();

            return entity;
        }
    }
}