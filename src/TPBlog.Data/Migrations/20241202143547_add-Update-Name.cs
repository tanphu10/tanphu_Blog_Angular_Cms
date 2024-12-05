using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class addUpdateName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskHistories",
                table: "TaskHistories");

            migrationBuilder.RenameTable(
                name: "TaskHistories",
                newName: "IC_TaskHistories");

            migrationBuilder.RenameIndex(
                name: "IX_TaskHistories_Slug",
                table: "IC_TaskHistories",
                newName: "IX_IC_TaskHistories_Slug");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IC_TaskHistories",
                table: "IC_TaskHistories",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IC_TaskHistories",
                table: "IC_TaskHistories");

            migrationBuilder.RenameTable(
                name: "IC_TaskHistories",
                newName: "TaskHistories");

            migrationBuilder.RenameIndex(
                name: "IX_IC_TaskHistories_Slug",
                table: "TaskHistories",
                newName: "IX_TaskHistories_Slug");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskHistories",
                table: "TaskHistories",
                column: "Id");
        }
    }
}
