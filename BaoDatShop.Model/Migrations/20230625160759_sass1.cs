using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class sass1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Datetime",
                table: "HistoryAccount",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Datetime",
                table: "HistoryAccount");
        }
    }
}
