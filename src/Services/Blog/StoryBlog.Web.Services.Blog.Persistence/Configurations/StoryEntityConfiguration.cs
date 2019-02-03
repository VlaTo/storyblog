using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Services.Blog.Persistence.Models;

namespace StoryBlog.Web.Services.Blog.Persistence.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class StoryEntityConfiguration : IEntityTypeConfiguration<Story>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            builder.HasIndex(story => story.Slug).IsUnique();
        }
    }
}