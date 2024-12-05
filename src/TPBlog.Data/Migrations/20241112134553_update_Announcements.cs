using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_Announcements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_Users_AppUserId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnouncementUsers_Announcements_AnnouncementId",
                table: "AnnouncementUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AnnouncementUsers_Users_UserId",
                table: "AnnouncementUsers");

            migrationBuilder.DropIndex(
                name: "IX_AnnouncementUsers_AnnouncementId",
                table: "AnnouncementUsers");

            migrationBuilder.DropIndex(
                name: "IX_Announcements_AppUserId",
                table: "Announcements");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Announcements",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Announcements",
                newName: "ID");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "Announcements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_AnnouncementUsers_AnnouncementId",
                table: "AnnouncementUsers",
                column: "AnnouncementId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_AppUserId",
                table: "Announcements",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_Users_AppUserId",
                table: "Announcements",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnouncementUsers_Announcements_AnnouncementId",
                table: "AnnouncementUsers",
                column: "AnnouncementId",
                principalTable: "Announcements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AnnouncementUsers_Users_UserId",
                table: "AnnouncementUsers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
