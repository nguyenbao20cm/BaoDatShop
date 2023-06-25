using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BaoDatShop.Model.Migrations
{
    public partial class sass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HistoryAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoryAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoryAccount_Account_AccountID",
                        column: x => x.AccountID,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoryAccount_AccountID",
                table: "HistoryAccount",
                column: "AccountID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoryAccount");
        }
    }
}
