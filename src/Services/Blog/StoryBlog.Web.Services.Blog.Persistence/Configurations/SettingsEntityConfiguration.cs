using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Services.Blog.Persistence.Models;

namespace StoryBlog.Web.Services.Blog.Persistence.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SettingsEntityConfiguration : IEntityTypeConfiguration<Settings>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Settings> builder)
        {
            builder.HasIndex(settings => settings.Name).IsUnique(unique: true);
        }
    }
}