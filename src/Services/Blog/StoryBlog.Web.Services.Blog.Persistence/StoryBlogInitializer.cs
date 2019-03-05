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
    }
}