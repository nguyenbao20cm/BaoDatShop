using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KHoHang_ProductSize_ProductSizeId",
                table: "KHoHang");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KHoHang",
                table: "KHoHang");

            migrationBuilder.RenameTable(
                name: "KHoHang",
                newName: "Warehouse");

            migrationBuilder.RenameIndex(
                name: "IX_KHoHang_ProductSizeId",
                table: "Warehouse",
                newName: "IX_Warehouse_ProductSizeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Warehouse_ProductSize_ProductSizeId",
                table: "Warehouse",
                column: "ProductSizeId",
                principalTable: "ProductSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Warehouse_ProductSize_ProductSizeId",
                table: "Warehouse");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouse",
                table: "Warehouse");

            migrationBuilder.RenameTable(
                name: "Warehouse",
                newName: "KHoHang");

            migrationBuilder.RenameIndex(
                name: "IX_Warehouse_ProductSizeId",
                table: "KHoHang",
                newName: "IX_KHoHang_ProductSizeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KHoHang",
                table: "KHoHang",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KHoHang_ProductSize_ProductSizeId",
                table: "KHoHang",
                column: "ProductSizeId",
                principalTable: "ProductSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
