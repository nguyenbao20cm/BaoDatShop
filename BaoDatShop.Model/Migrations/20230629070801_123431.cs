using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class _123431 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "Supplier",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Supplier");
        }
    }
}
