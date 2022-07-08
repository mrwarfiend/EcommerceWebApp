using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceProject.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sale",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    subtotal = table.Column<float>(type: "real", nullable: false),
                    tax = table.Column<float>(type: "real", nullable: false),
                    total = table.Column<float>(type: "real", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sale", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    desc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categId = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<float>(type: "real", nullable: false),
                    tax = table.Column<float>(type: "real", nullable: false),
                    img = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_category_categId",
                        column: x => x.categId,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    prodId = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false),
                    subtotal = table.Column<float>(type: "real", nullable: false),
                    tax = table.Column<float>(type: "real", nullable: false),
                    total = table.Column<float>(type: "real", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart", x => x.id);
                    table.ForeignKey(
                        name: "FK_cart_product_prodId",
                        column: x => x.prodId,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prodId = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false),
                    reorderLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => x.id);
                    table.ForeignKey(
                        name: "FK_inventory_product_prodId",
                        column: x => x.prodId,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productsSold",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prodId = table.Column<int>(type: "int", nullable: false),
                    count = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<float>(type: "real", nullable: false),
                    tax = table.Column<float>(type: "real", nullable: false),
                    totalPrice = table.Column<float>(type: "real", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productsSold", x => x.id);
                    table.ForeignKey(
                        name: "FK_productsSold_product_prodId",
                        column: x => x.prodId,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_prodId",
                table: "cart",
                column: "prodId");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_prodId",
                table: "inventory",
                column: "prodId");

            migrationBuilder.CreateIndex(
                name: "IX_product_categId",
                table: "product",
                column: "categId");

            migrationBuilder.CreateIndex(
                name: "IX_productsSold_prodId",
                table: "productsSold",
                column: "prodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart");

            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "productsSold");

            migrationBuilder.DropTable(
                name: "sale");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "category");
        }
    }
}
