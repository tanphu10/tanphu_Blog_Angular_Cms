using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatetasknotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IC_TaskNotificationUsers",
                table: "IC_TaskNotificationUsers");

            migrationBuilder.DropColumn(
                name: "DateLastModified",
                table: "IC_TaskNotificationUsers");

            migrationBuilder.DropColumn(
                name: "HasRead",
                table: "IC_TaskNotifications");

            migrationBuilder.RenameColumn(
                name: "TaskNotificationId",
                table: "IC_TaskNotificationUsers",
                newName: "TaskId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "IC_TaskNotificationUsers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "IC_TaskNotifications",
                newName: "UserName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateCreated",
                table: "IC_TaskNotificationUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "IC_TaskNotificationUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "IC_TaskNotifications",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_IC_TaskNotificationUsers",
                table: "IC_TaskNotificationUsers",
                columns: new[] { "UserId", "TaskId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IC_TaskNotificationUsers",
                table: "IC_TaskNotificationUsers");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "IC_TaskNotificationUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IC_TaskNotifications");

            migrationBuilder.RenameColumn(
                name: "TaskId",
                table: "IC_TaskNotificationUsers",
                newName: "TaskNotificationId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "IC_TaskNotificationUsers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "IC_TaskNotifications",
                newName: "Content");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "DateCreated",
                table: "IC_TaskNotificationUsers",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateLastModified",
                table: "IC_TaskNotificationUsers",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasRead",
                table: "IC_TaskNotifications",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IC_TaskNotificationUsers",
                table: "IC_TaskNotificationUsers",
                column: "Id");
        }
    }
}
