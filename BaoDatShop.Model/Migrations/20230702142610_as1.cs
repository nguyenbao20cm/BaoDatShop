using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class as1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Voucher");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Voucher",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
