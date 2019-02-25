using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Services.Blog.Persistence.Models;

namespace StoryBlog.Web.Services.Blog.Persistence.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class FeaturedEntityConfiguration : IEntityTypeConfiguration<Featured>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<Featured> builder)
        {
            builder.HasIndex(featured => featured.StoryId).IsUnique(unique: true);
        }
    }
}