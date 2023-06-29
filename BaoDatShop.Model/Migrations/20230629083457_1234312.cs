using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class _1234312 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaxCode",
                table: "Supplier",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxCode",
                table: "Supplier");
        }
    }
}
