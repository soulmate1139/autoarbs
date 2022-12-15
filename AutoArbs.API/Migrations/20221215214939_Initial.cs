using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoArbs.API.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDeposit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalWithdrawal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalBonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "DepositHistories",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Deposit_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositHistories", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_DepositHistories_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.CreateTable(
                name: "WithdrawalHistories",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Withdrawal_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account_withdrawn_to = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalHistories", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_WithdrawalHistories_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "Balance", "Bonus", "FirstName", "IsActive", "LastName", "Password", "TotalBonus", "TotalDeposit", "TotalWithdrawal" },
                values: new object[] { "john@gmail.com", 0m, 0m, "John", false, "Doe", "John123", 0m, 0m, 0m });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "Balance", "Bonus", "FirstName", "IsActive", "LastName", "Password", "TotalBonus", "TotalDeposit", "TotalWithdrawal" },
                values: new object[] { "mary@gmail.com", 50m, 0m, "Mary", false, "Jane", "mary123", 0m, 0m, 0m });

            migrationBuilder.CreateIndex(
                name: "IX_DepositHistories_UserEmail",
                table: "DepositHistories",
                column: "UserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalHistories_UserEmail",
                table: "WithdrawalHistories",
                column: "UserEmail");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DepositHistories");

            migrationBuilder.DropTable(
                name: "WithdrawalHistories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
