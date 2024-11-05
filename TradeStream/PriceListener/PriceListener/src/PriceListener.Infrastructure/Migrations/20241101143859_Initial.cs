using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PriceListener.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderBookDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Timestamp = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    Microtimestamp = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBookDatas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Asks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderBookDataId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asks_OrderBookDatas_OrderBookDataId",
                        column: x => x.OrderBookDataId,
                        principalTable: "OrderBookDatas",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrderBookDataId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Price = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bids_OrderBookDatas_OrderBookDataId",
                        column: x => x.OrderBookDataId,
                        principalTable: "OrderBookDatas",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OrderBooks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DataId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Channel = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Event = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderBooks_OrderBookDatas_DataId",
                        column: x => x.DataId,
                        principalTable: "OrderBookDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Asks_OrderBookDataId",
                table: "Asks",
                column: "OrderBookDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Bids_OrderBookDataId",
                table: "Bids",
                column: "OrderBookDataId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderBooks_DataId",
                table: "OrderBooks",
                column: "DataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asks");

            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.DropTable(
                name: "OrderBooks");

            migrationBuilder.DropTable(
                name: "OrderBookDatas");
        }
    }
}
