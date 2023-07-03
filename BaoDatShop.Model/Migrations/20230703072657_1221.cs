using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class _1221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Link",
                table: "AdvertisingPanel",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "AdvertisingPanel",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "AdvertisingPanel",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AdvertisingPanel_ProductId",
                table: "AdvertisingPanel",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvertisingPanel_Product_ProductId",
                table: "AdvertisingPanel",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvertisingPanel_Product_ProductId",
                table: "AdvertisingPanel");

            migrationBuilder.DropIndex(
                name: "IX_AdvertisingPanel_ProductId",
                table: "AdvertisingPanel");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "AdvertisingPanel");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "AdvertisingPanel");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "AdvertisingPanel",
                newName: "Link");
        }
    }
}
