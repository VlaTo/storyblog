using Microsoft.EntityFrameworkCore;

namespace StoryBlog.Web.Services.Identity.API.Data
{
    public sealed class StoryBlogIdentityDbContextFactory : DesignTimeDbContextFactoryBase<StoryBlogIdentityDbContext>
    {
        public StoryBlogIdentityDbContextFactory()
            : base("StoryBlog", typeof(Program).Assembly.GetName().Name)
        {
        }

        protected override StoryBlogIdentityDbContext CreateNewInstance(DbContextOptions<StoryBlogIdentityDbContext> options)
        {
            return new StoryBlogIdentityDbContext(options);
        }

        /*public StoryBlogIdentityDbContext CreateDbContext(string[] args)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../../../");
            var configuration = new ConfigurationBuilder()
                .SetBasePath(path)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            var context = new DbContextOptionsBuilder<StoryBlogIdentityDbContext>();
            var connectionString = configuration.GetConnectionString("StoryBlog");

            context.UseSqlite(connectionString, options =>
            {
                options.MigrationsAssembly(typeof(Program).Assembly.GetName().Name);
            });

            return new StoryBlogIdentityDbContext(context.Options);
        }*/
    }
}