using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StoryBlog.Web.Services.Blog.Persistence.Models;

namespace StoryBlog.Web.Services.Blog.Persistence.Configurations
{
    public sealed class RubricEntityConfiguration : IEntityTypeConfiguration<Rubric>
    {
        public void Configure(EntityTypeBuilder<Rubric> builder)
        {
            builder.HasIndex(rubric => rubric.Slug).IsUnique();
        }
    }
}