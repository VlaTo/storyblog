using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Services.Blog.Persistence.Models;

namespace StoryBlog.Web.Services.Blog.Persistence
{
    /// <summary>
    /// 
    /// </summary>
    public class StoryBlogDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Address> Addresses
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Author> Authors
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Story> Stories
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Comment> Comments
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public StoryBlogDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoryBlogDbContext).Assembly);
        }
    }
}