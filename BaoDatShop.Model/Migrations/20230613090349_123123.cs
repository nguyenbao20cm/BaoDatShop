using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class _123123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportPrice",
                table: "Product");

            migrationBuilder.AddColumn<int>(
                name: "ImportPrice",
                table: "ProductSize",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "IssuedDate",
                table: "ProductSize",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportPrice",
                table: "ProductSize");

            migrationBuilder.DropColumn(
                name: "IssuedDate",
                table: "ProductSize");

            migrationBuilder.AddColumn<int>(
                name: "ImportPrice",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
