using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatetasknotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "IC_TaskNotificationUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "IC_TaskNotifications",
                newName: "UserBy");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "IC_TaskNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProjectSlug",
                table: "IC_TaskNotifications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "IC_TaskNotifications");

            migrationBuilder.DropColumn(
                name: "ProjectSlug",
                table: "IC_TaskNotifications");

            migrationBuilder.RenameColumn(
                name: "UserBy",
                table: "IC_TaskNotifications",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "IC_TaskNotificationUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
