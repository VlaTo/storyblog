﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoryBlog.Web.Services.Blog.Persistence;

namespace StoryBlog.Web.Services.Blog.Persistence.Migrations
{
    [DbContext(typeof(StoryBlogDbContext))]
    partial class StoryBlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Address", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AddressTypes");

                    b.Property<long>("AuthorId");

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<string>("State");

                    b.Property<string>("Street");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Author", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AuthorId");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsPublic");

                    b.Property<DateTime?>("Modified");

                    b.Property<long?>("ParentId");

                    b.Property<int>("Status");

                    b.Property<long>("StoryId");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ParentId");

                    b.HasIndex("StoryId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Featured", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("StoryId");

                    b.HasKey("Id");

                    b.HasIndex("StoryId")
                        .IsUnique();

                    b.ToTable("Featured");
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Rubric", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("Order");

                    b.Property<string>("Slug");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Rubrics");
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Settings", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Value")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Story", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AuthorId");

                    b.Property<string>("Content");

                    b.Property<DateTime>("Created");

                    b.Property<bool>("IsPublic");

                    b.Property<DateTime?>("Modified");

                    b.Property<DateTime?>("Published");

                    b.Property<string>("Slug");

                    b.Property<int>("Status");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Address", b =>
                {
                    b.HasOne("StoryBlog.Web.Services.Blog.Persistence.Models.Author", "Author")
                        .WithMany("Addresses")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Comment", b =>
                {
                    b.HasOne("StoryBlog.Web.Services.Blog.Persistence.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StoryBlog.Web.Services.Blog.Persistence.Models.Comment", "Parent")
                        .WithMany("Comments")
                        .HasForeignKey("ParentId");

                    b.HasOne("StoryBlog.Web.Services.Blog.Persistence.Models.Story", "Story")
                        .WithMany("Comments")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Featured", b =>
                {
                    b.HasOne("StoryBlog.Web.Services.Blog.Persistence.Models.Story", "Story")
                        .WithMany()
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StoryBlog.Web.Services.Blog.Persistence.Models.Story", b =>
                {
                    b.HasOne("StoryBlog.Web.Services.Blog.Persistence.Models.Author", "Author")
                        .WithMany("Stories")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
