using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateinvtIC_Announcement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AnnouncementUsers",
                table: "AnnouncementUsers");

            migrationBuilder.RenameTable(
                name: "AnnouncementUsers",
                newName: "IC_AnnouncementUsers");

            migrationBuilder.AlterColumn<string>(
                name: "StkUnit",
                table: "IC_InventoryEntries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IC_AnnouncementUsers",
                table: "IC_AnnouncementUsers",
                columns: new[] { "UserId", "AnnouncementId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_IC_AnnouncementUsers",
                table: "IC_AnnouncementUsers");

            migrationBuilder.RenameTable(
                name: "IC_AnnouncementUsers",
                newName: "AnnouncementUsers");

            migrationBuilder.AlterColumn<int>(
                name: "StkUnit",
                table: "IC_InventoryEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AnnouncementUsers",
                table: "AnnouncementUsers",
                columns: new[] { "UserId", "AnnouncementId" });
        }
    }
}
