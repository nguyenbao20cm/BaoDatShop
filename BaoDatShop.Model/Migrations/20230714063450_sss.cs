using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class sss : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VoucherId",
                table: "Invoice",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_VoucherId",
                table: "Invoice",
                column: "VoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Voucher_VoucherId",
                table: "Invoice",
                column: "VoucherId",
                principalTable: "Voucher",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Voucher_VoucherId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_VoucherId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "VoucherId",
                table: "Invoice");
        }
    }
}
