using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalaryProjectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Enterprise_EnterpriseId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Enterprise_Banks_BankId",
                table: "Enterprise");

            migrationBuilder.DropForeignKey(
                name: "FK_Installment_Banks_BankId",
                table: "Installment");

            migrationBuilder.DropForeignKey(
                name: "FK_Installment_Users_PayerId",
                table: "Installment");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Banks_BankId",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Users_BorrowerId",
                table: "Loan");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_Accounts_ReceiverId",
                table: "Transfer");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_Accounts_SenderId",
                table: "Transfer");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfer_Banks_BankId",
                table: "Transfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfer",
                table: "Transfer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loan",
                table: "Loan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Installment",
                table: "Installment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enterprise",
                table: "Enterprise");

            migrationBuilder.RenameTable(
                name: "Transfer",
                newName: "Transfers");

            migrationBuilder.RenameTable(
                name: "Loan",
                newName: "Loans");

            migrationBuilder.RenameTable(
                name: "Installment",
                newName: "Installments");

            migrationBuilder.RenameTable(
                name: "Enterprise",
                newName: "Enterprises");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_SenderId",
                table: "Transfers",
                newName: "IX_Transfers_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_ReceiverId",
                table: "Transfers",
                newName: "IX_Transfers_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfer_BankId",
                table: "Transfers",
                newName: "IX_Transfers_BankId");

            migrationBuilder.RenameIndex(
                name: "IX_Loan_BorrowerId",
                table: "Loans",
                newName: "IX_Loans_BorrowerId");

            migrationBuilder.RenameIndex(
                name: "IX_Loan_BankId",
                table: "Loans",
                newName: "IX_Loans_BankId");

            migrationBuilder.RenameIndex(
                name: "IX_Installment_PayerId",
                table: "Installments",
                newName: "IX_Installments_PayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Installment_BankId",
                table: "Installments",
                newName: "IX_Installments_BankId");

            migrationBuilder.RenameIndex(
                name: "IX_Enterprise_BankId",
                table: "Enterprises",
                newName: "IX_Enterprises_BankId");

            migrationBuilder.AddColumn<int>(
                name: "SalaryProjectId",
                table: "Accounts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loans",
                table: "Loans",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Installments",
                table: "Installments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enterprises",
                table: "Enterprises",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SalaryProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnterpriseId = table.Column<int>(type: "INTEGER", nullable: false),
                    EnterpriseAccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryProjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryProjects_Accounts_EnterpriseAccountId",
                        column: x => x.EnterpriseAccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryProjects_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_SalaryProjectId",
                table: "Accounts",
                column: "SalaryProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryProjects_EnterpriseAccountId",
                table: "SalaryProjects",
                column: "EnterpriseAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryProjects_EnterpriseId",
                table: "SalaryProjects",
                column: "EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Enterprises_EnterpriseId",
                table: "Accounts",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_SalaryProjects_SalaryProjectId",
                table: "Accounts",
                column: "SalaryProjectId",
                principalTable: "SalaryProjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enterprises_Banks_BankId",
                table: "Enterprises",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_Banks_BankId",
                table: "Installments",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_Users_PayerId",
                table: "Installments",
                column: "PayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Banks_BankId",
                table: "Loans",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Users_BorrowerId",
                table: "Loans",
                column: "BorrowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_ReceiverId",
                table: "Transfers",
                column: "ReceiverId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_SenderId",
                table: "Transfers",
                column: "SenderId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Banks_BankId",
                table: "Transfers",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Enterprises_EnterpriseId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_SalaryProjects_SalaryProjectId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Enterprises_Banks_BankId",
                table: "Enterprises");

            migrationBuilder.DropForeignKey(
                name: "FK_Installments_Banks_BankId",
                table: "Installments");

            migrationBuilder.DropForeignKey(
                name: "FK_Installments_Users_PayerId",
                table: "Installments");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Banks_BankId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Users_BorrowerId",
                table: "Loans");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_ReceiverId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_SenderId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Banks_BankId",
                table: "Transfers");

            migrationBuilder.DropTable(
                name: "SalaryProjects");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_SalaryProjectId",
                table: "Accounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transfers",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Loans",
                table: "Loans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Installments",
                table: "Installments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Enterprises",
                table: "Enterprises");

            migrationBuilder.DropColumn(
                name: "SalaryProjectId",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Transfers",
                newName: "Transfer");

            migrationBuilder.RenameTable(
                name: "Loans",
                newName: "Loan");

            migrationBuilder.RenameTable(
                name: "Installments",
                newName: "Installment");

            migrationBuilder.RenameTable(
                name: "Enterprises",
                newName: "Enterprise");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_SenderId",
                table: "Transfer",
                newName: "IX_Transfer_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_ReceiverId",
                table: "Transfer",
                newName: "IX_Transfer_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_Transfers_BankId",
                table: "Transfer",
                newName: "IX_Transfer_BankId");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_BorrowerId",
                table: "Loan",
                newName: "IX_Loan_BorrowerId");

            migrationBuilder.RenameIndex(
                name: "IX_Loans_BankId",
                table: "Loan",
                newName: "IX_Loan_BankId");

            migrationBuilder.RenameIndex(
                name: "IX_Installments_PayerId",
                table: "Installment",
                newName: "IX_Installment_PayerId");

            migrationBuilder.RenameIndex(
                name: "IX_Installments_BankId",
                table: "Installment",
                newName: "IX_Installment_BankId");

            migrationBuilder.RenameIndex(
                name: "IX_Enterprises_BankId",
                table: "Enterprise",
                newName: "IX_Enterprise_BankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transfer",
                table: "Transfer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Loan",
                table: "Loan",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Installment",
                table: "Installment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Enterprise",
                table: "Enterprise",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Enterprise_EnterpriseId",
                table: "Accounts",
                column: "EnterpriseId",
                principalTable: "Enterprise",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Enterprise_Banks_BankId",
                table: "Enterprise",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Installment_Banks_BankId",
                table: "Installment",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Installment_Users_PayerId",
                table: "Installment",
                column: "PayerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Banks_BankId",
                table: "Loan",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Users_BorrowerId",
                table: "Loan",
                column: "BorrowerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_Accounts_ReceiverId",
                table: "Transfer",
                column: "ReceiverId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_Accounts_SenderId",
                table: "Transfer",
                column: "SenderId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfer_Banks_BankId",
                table: "Transfer",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id");
        }
    }
}
