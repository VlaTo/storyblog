using Microsoft.EntityFrameworkCore.Migrations;

namespace StoryBlog.Web.Services.Identity.Persistence.Migrations
{
    public partial class CustomerContactNameAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactName",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactName",
                table: "AspNetUsers");
        }
    }
}
