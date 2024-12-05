using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class addconversionunit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BaseUnit",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CnvFact",
                table: "InventoryEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "POUnit",
                table: "InventoryEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SOUnit",
                table: "InventoryEntries",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StkUnit",
                table: "InventoryEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseUnit",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CnvFact",
                table: "InventoryEntries");

            migrationBuilder.DropColumn(
                name: "POUnit",
                table: "InventoryEntries");

            migrationBuilder.DropColumn(
                name: "SOUnit",
                table: "InventoryEntries");

            migrationBuilder.DropColumn(
                name: "StkUnit",
                table: "InventoryEntries");
        }
    }
}
