using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EcommerceProject.Migrations
{
    public partial class AddedCheckForCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "check",
                table: "cart",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "check",
                table: "cart");
        }
    }
}
