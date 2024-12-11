using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSlugTaskHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IC_TaskHistories_Slug",
                table: "IC_TaskHistories");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "IC_TaskHistories",
                newName: "TaskSlug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskSlug",
                table: "IC_TaskHistories",
                newName: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_IC_TaskHistories_Slug",
                table: "IC_TaskHistories",
                column: "Slug",
                unique: true);
        }
    }
}
