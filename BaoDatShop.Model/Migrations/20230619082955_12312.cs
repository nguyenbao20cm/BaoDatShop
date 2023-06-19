    using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class _12312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_Product_ProductId",
                table: "InvoiceDetail");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "InvoiceDetail",
                newName: "ProductSizeId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetail_ProductId",
                table: "InvoiceDetail",
                newName: "IX_InvoiceDetail_ProductSizeId");

            migrationBuilder.CreateTable(
                name: "FavoriteProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteProduct_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FavoriteProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Voucher",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Disscount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voucher", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProduct_AccountId",
                table: "FavoriteProduct",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProduct_ProductId",
                table: "FavoriteProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Voucher_Name",
                table: "Voucher",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_ProductSize_ProductSizeId",
                table: "InvoiceDetail",
                column: "ProductSizeId",
                principalTable: "ProductSize",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceDetail_ProductSize_ProductSizeId",
                table: "InvoiceDetail");

            migrationBuilder.DropTable(
                name: "FavoriteProduct");

            migrationBuilder.DropTable(
                name: "Voucher");

            migrationBuilder.RenameColumn(
                name: "ProductSizeId",
                table: "InvoiceDetail",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_InvoiceDetail_ProductSizeId",
                table: "InvoiceDetail",
                newName: "IX_InvoiceDetail_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceDetail_Product_ProductId",
                table: "InvoiceDetail",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
