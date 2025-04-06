using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyWardrobeStatistics.Migrations
{
    /// <inheritdoc />
    public partial class ChangedProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WardrobeItemsStatistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Subcategory = table.Column<string>(type: "TEXT", nullable: false),
                    WardrobeItemUsage = table.Column<int>(type: "INTEGER", nullable: false),
                    LastTimeUsed = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WardrobeItemsStatistics", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WardrobeItemsStatistics");
        }
    }
}
