using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class addint1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Disscount_ProductId",
                table: "Disscount");

            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "ProductSize",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Disscount_ProductId",
                table: "Disscount",
                column: "ProductId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Disscount_ProductId",
                table: "Disscount");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ProductSize",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.CreateIndex(
                name: "IX_Disscount_ProductId",
                table: "Disscount",
                column: "ProductId");
        }
    }
}
