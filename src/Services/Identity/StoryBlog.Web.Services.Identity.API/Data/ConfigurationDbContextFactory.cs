using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace StoryBlog.Web.Services.Identity.API.Data
{
    public sealed class ConfigurationDbContextFactory : DesignTimeDbContextFactoryBase<ConfigurationDbContext>
    {
        public ConfigurationDbContextFactory() 
            : base("StoryBlog", typeof(Program).Assembly.GetName().Name)
        {
        }

        protected override ConfigurationDbContext CreateNewInstance(DbContextOptions<ConfigurationDbContext> options)
        {
            return new ConfigurationDbContext(options, new ConfigurationStoreOptions());
        }
    }
}