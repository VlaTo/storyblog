using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Services.Shared.Persistence;

namespace StoryBlog.Web.Services.Identity.Persistence
{
    public sealed class ConfigurationDbContextFactory : DesignTimeDbContextFactoryBase<ConfigurationDbContext>
    {
        public ConfigurationDbContextFactory() 
            : base("StoryBlog", typeof(StoryBlogIdentityDbContext).Assembly.GetName().Name)
        {
        }

        protected override ConfigurationDbContext CreateNewInstance(DbContextOptions<ConfigurationDbContext> options)
        {
            return new ConfigurationDbContext(options, new ConfigurationStoreOptions());
        }
    }
}