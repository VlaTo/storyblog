using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace StoryBlog.Web.Services.Identity.API.Data
{
    public sealed class PersistedGrantDbContextFactory : DesignTimeDbContextFactoryBase<PersistedGrantDbContext>
    {
        public PersistedGrantDbContextFactory()
            : base("StoryBlog", typeof(Program).Assembly.GetName().Name)
        {
        }

        protected override PersistedGrantDbContext CreateNewInstance(DbContextOptions<PersistedGrantDbContext> options)
        {
            return new PersistedGrantDbContext(options, new OperationalStoreOptions());
        }
    }
}