﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StoryBlog.Web.Services.Identity.API.Data.Models;

namespace StoryBlog.Web.Services.Identity.API.Data
{
    public class StoryBlogIdentityDbContext : IdentityDbContext<Customer, CustomerRole, long>
    {
        public DbSet<Address> Addresses
        {
            get;
            protected set;
        }

        public DbSet<Card> Cards
        {
            get;
            set;
        }

        public StoryBlogIdentityDbContext(DbContextOptions<StoryBlogIdentityDbContext> options)
            : base(options)
        {
        }
    }
}