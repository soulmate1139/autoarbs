using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoArbs.API.Migrations
{
    public partial class update_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "WithdrawalHistories",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "DepositHistories",
                newName: "Username");

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "WithdrawalHistories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "DepositHistories",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalHistories_UserName",
                table: "WithdrawalHistories",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_DepositHistories_UserName",
                table: "DepositHistories",
                column: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_DepositHistories_Users_UserName",
                table: "DepositHistories",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName");

            migrationBuilder.AddForeignKey(
                name: "FK_WithdrawalHistories_Users_UserName",
                table: "WithdrawalHistories",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepositHistories_Users_UserName",
                table: "DepositHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_WithdrawalHistories_Users_UserName",
                table: "WithdrawalHistories");

            migrationBuilder.DropIndex(
                name: "IX_WithdrawalHistories_UserName",
                table: "WithdrawalHistories");

            migrationBuilder.DropIndex(
                name: "IX_DepositHistories_UserName",
                table: "DepositHistories");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "WithdrawalHistories");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "DepositHistories");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "WithdrawalHistories",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "DepositHistories",
                newName: "UserName");
        }
    }
}
