using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoArbs.API.Migrations
{
    public partial class InitialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Bonus = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "DepositHistories",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Deposit_Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepositHistories", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_DepositHistories_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName");
                });

            migrationBuilder.CreateTable(
                name: "WithdrawalHistories",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Withdrawal_Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Account_withdrawn_to = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WithdrawalHistories", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_WithdrawalHistories_Users_UserName",
                        column: x => x.UserName,
                        principalTable: "Users",
                        principalColumn: "UserName");
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserName", "Balance", "Bonus", "Email", "FirstName", "IsActive", "LastName", "Password" },
                values: new object[] { "John1", 0m, 0m, "john@gmail.com", "John", false, "Doe", "John123" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserName", "Balance", "Bonus", "Email", "FirstName", "IsActive", "LastName", "Password" },
                values: new object[] { "Mary1", 50m, 0m, "mary@gmail.com", "Mary", false, "Jane", "mary123" });

            migrationBuilder.CreateIndex(
                name: "IX_DepositHistories_UserName",
                table: "DepositHistories",
                column: "UserName");

            migrationBuilder.CreateIndex(
                name: "IX_WithdrawalHistories_UserName",
                table: "WithdrawalHistories",
                column: "UserName");
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
