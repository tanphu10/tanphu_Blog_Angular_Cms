using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStartDateTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "oldContent",
                table: "IC_TaskHistories",
                newName: "OldContent");

            migrationBuilder.RenameColumn(
                name: "newContent",
                table: "IC_TaskHistories",
                newName: "NewContent");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                table: "IC_Tasks",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "IC_Tasks");

            migrationBuilder.RenameColumn(
                name: "OldContent",
                table: "IC_TaskHistories",
                newName: "oldContent");

            migrationBuilder.RenameColumn(
                name: "NewContent",
                table: "IC_TaskHistories",
                newName: "newContent");
        }
    }
}
