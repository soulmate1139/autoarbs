using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoArbs.API.Migrations
{
    public partial class new_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WithdrawalHistories",
                table: "WithdrawalHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepositHistories",
                table: "DepositHistories");

            migrationBuilder.RenameColumn(
                name: "RecipientToAccount",
                table: "WithdrawalHistories",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                table: "WithdrawalHistories",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedAt",
                table: "DepositHistories",
                newName: "UpdateAt");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "WithdrawalHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Account_withdrawn_to",
                table: "WithdrawalHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "DepositHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "DepositHistories",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WithdrawalHistories",
                table: "WithdrawalHistories",
                column: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepositHistories",
                table: "DepositHistories",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WithdrawalHistories",
                table: "WithdrawalHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepositHistories",
                table: "DepositHistories");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "WithdrawalHistories");

            migrationBuilder.DropColumn(
                name: "Account_withdrawn_to",
                table: "WithdrawalHistories");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "DepositHistories");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "DepositHistories");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "WithdrawalHistories",
                newName: "LastUpdatedAt");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "WithdrawalHistories",
                newName: "RecipientToAccount");

            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "DepositHistories",
                newName: "LastUpdatedAt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WithdrawalHistories",
                table: "WithdrawalHistories",
                column: "UserName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepositHistories",
                table: "DepositHistories",
                column: "UserName");
        }
    }
}
