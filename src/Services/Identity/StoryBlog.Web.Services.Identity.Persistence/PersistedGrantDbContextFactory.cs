using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Services.Shared.Persistence;

namespace StoryBlog.Web.Services.Identity.Persistence
{
    public sealed class PersistedGrantDbContextFactory : DesignTimeDbContextFactoryBase<PersistedGrantDbContext>
    {
        public PersistedGrantDbContextFactory()
            : base("StoryBlog", typeof(StoryBlogIdentityDbContext).Assembly.GetName().Name)
        {
        }

        protected override PersistedGrantDbContext CreateNewInstance(DbContextOptions<PersistedGrantDbContext> options)
        {
            return new PersistedGrantDbContext(options, new OperationalStoreOptions());
        }
    }
}