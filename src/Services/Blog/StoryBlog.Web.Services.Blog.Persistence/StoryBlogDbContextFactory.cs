using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace StoryBlog.Web.Services.Blog.Persistence
{
    public sealed class StoryBlogDbContextFactory : IDesignTimeDbContextFactory<StoryBlogDbContext>
    {
        public StoryBlogDbContext CreateDbContext(string[] args)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../../StoryBlog.Web.Services.Blog.API/");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.Development.json", optional: true)
                .Build();
            var context = new DbContextOptionsBuilder<StoryBlogDbContext>();
            var connectionString = configuration.GetConnectionString("StoryBlog");

            context.UseSqlite(connectionString, options =>
            {
                options.MigrationsAssembly(typeof(StoryBlogDbContext).Assembly.GetName().Name);
            });

            return new StoryBlogDbContext(context.Options);
        }
    }
}