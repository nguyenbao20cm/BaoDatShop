using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class âsss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LinkProduct",
                table: "AdvertisingPanel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LinkProductType",
                table: "AdvertisingPanel",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkProduct",
                table: "AdvertisingPanel");

            migrationBuilder.DropColumn(
                name: "LinkProductType",
                table: "AdvertisingPanel");
        }
    }
}
