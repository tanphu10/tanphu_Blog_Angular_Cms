using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class addprojectslug : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "Tasks",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "TaskHistories",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "TaskComments",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "TaskAttachments",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "Tags",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "Series",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "Posts",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "PostCategories",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "PostActivityLogs",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "InventoryEntries",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "InventoryCategories",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "Announcements",
                type: "varchar(250)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "TaskHistories");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "TaskAttachments");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "PostCategories");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "PostActivityLogs");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "InventoryEntries");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "InventoryCategories");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "Announcements");
        }
    }
}
