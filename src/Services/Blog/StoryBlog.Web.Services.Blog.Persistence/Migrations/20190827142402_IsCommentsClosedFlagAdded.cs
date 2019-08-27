using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryBlog.Web.Services.Blog.Persistence.Migrations
{
    public partial class IsCommentsClosedFlagAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCommentsClosed",
                table: "Stories",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCommentsClosed",
                table: "Stories");
        }
    }
}
