using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceSimulator.Infrastructure.Migrations
{
    public partial class PriceSimulatorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PriceSimulations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Quantity = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Cryptocurrency = table.Column<int>(type: "int", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceSimulations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    PriceSimulationId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_PriceSimulations_PriceSimulationId",
                        column: x => x.PriceSimulationId,
                        principalTable: "PriceSimulations",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_PriceSimulationId",
                table: "Prices",
                column: "PriceSimulationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "PriceSimulations");
        }
    }
}
