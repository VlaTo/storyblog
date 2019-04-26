using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryBlog.Web.Services.Blog.Persistence.Migrations
{
    public partial class StoryPublishedAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Published",
                table: "Stories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "Stories");
        }
    }
}
