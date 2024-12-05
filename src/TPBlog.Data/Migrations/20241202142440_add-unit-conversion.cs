using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPBlog.Data.Migrations
{
    /// <inheritdoc />
    public partial class addunitconversion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IC_UnitConversion",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InvtId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromUnit = table.Column<string>(type: "varchar(250)", nullable: false),
                    ToUnit = table.Column<string>(type: "varchar(250)", nullable: false),
                    CnvFactor = table.Column<int>(type: "int", nullable: false),
                    MultDiv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FromUnitDescr = table.Column<string>(type: "varchar(250)", nullable: false),
                    ToUnitDescr = table.Column<string>(type: "varchar(250)", nullable: false),
                    DateCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateLastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IC_UnitConversion", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IC_UnitConversion");
        }
    }
}
