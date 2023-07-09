using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class s : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Level",
                table: "Account");

            migrationBuilder.AlterColumn<string>(
                name: "DateTime",
                table: "VnpayBill",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "CardType",
                table: "VnpayBill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeBank",
                table: "VnpayBill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceBankID",
                table: "VnpayBill",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardType",
                table: "VnpayBill");

            migrationBuilder.DropColumn(
                name: "CodeBank",
                table: "VnpayBill");

            migrationBuilder.DropColumn(
                name: "InvoiceBankID",
                table: "VnpayBill");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "VnpayBill",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Account",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
