using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class _1232 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisingPanel_Product_ProductId",
                table: "AdvertisingPanel");

            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisingPanel_ProductType_ProductTypeId",
                table: "AdvertisingPanel");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisingPanel_ProductId",
                table: "AdvertisingPanel");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisingPanel_ProductTypeId",
                table: "AdvertisingPanel");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AdvertisingPanel");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "AdvertisingPanel");

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Footer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Footer",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "AdvertisingPanel",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Footer");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Footer");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "AdvertisingPanel");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "AdvertisingPanel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductTypeId",
                table: "AdvertisingPanel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisingPanel_ProductId",
                table: "AdvertisingPanel",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisingPanel_ProductTypeId",
                table: "AdvertisingPanel",
                column: "ProductTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisingPanel_Product_ProductId",
                table: "AdvertisingPanel",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisingPanel_ProductType_ProductTypeId",
                table: "AdvertisingPanel",
                column: "ProductTypeId",
                principalTable: "ProductType",
                principalColumn: "Id");
        }
    }
}
