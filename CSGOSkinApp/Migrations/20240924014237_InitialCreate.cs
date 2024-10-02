using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CSGOSkinApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Skins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<int>(type: "INTEGER", nullable: false),
                    Weapon = table.Column<string>(type: "TEXT", nullable: false),
                    Condition = table.Column<string>(type: "TEXT", nullable: false),
                    Float = table.Column<float>(type: "REAL", nullable: false),
                    MarketPrice = table.Column<decimal>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    dateListed = table.Column<DateTime>(type: "TEXT", nullable: false),
                    sticker1 = table.Column<string>(type: "TEXT", nullable: true),
                    sticker2 = table.Column<string>(type: "TEXT", nullable: true),
                    sticker3 = table.Column<string>(type: "TEXT", nullable: true),
                    sticker4 = table.Column<string>(type: "TEXT", nullable: true),
                    sticker5 = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skins", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Skins");
        }
    }
}
