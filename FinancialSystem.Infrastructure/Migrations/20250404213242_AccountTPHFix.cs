using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AccountTPHFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Banks_BankId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Enterprises_EnterpriseId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Enterprises_UserAccount_EnterpriseId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Users_OwnerId",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjectEmployees_Account_UserAccountId",
                table: "SalaryProjectEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjects_Account_EnterpriseAccountId",
                table: "SalaryProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Account_ReceiverId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Account_SenderId",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Account",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_Account_UserAccount_EnterpriseId",
                table: "Accounts",
                newName: "IX_Accounts_UserAccount_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_OwnerId",
                table: "Accounts",
                newName: "IX_Accounts_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_EnterpriseId",
                table: "Accounts",
                newName: "IX_Accounts_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_BankId",
                table: "Accounts",
                newName: "IX_Accounts_BankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Banks_BankId",
                table: "Accounts",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Enterprises_EnterpriseId",
                table: "Accounts",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Enterprises_UserAccount_EnterpriseId",
                table: "Accounts",
                column: "UserAccount_EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_OwnerId",
                table: "Accounts",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryProjectEmployees_Accounts_UserAccountId",
                table: "SalaryProjectEmployees",
                column: "UserAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryProjects_Accounts_EnterpriseAccountId",
                table: "SalaryProjects",
                column: "EnterpriseAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_ReceiverId",
                table: "Transfers",
                column: "ReceiverId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Accounts_SenderId",
                table: "Transfers",
                column: "SenderId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Banks_BankId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Enterprises_EnterpriseId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Enterprises_UserAccount_EnterpriseId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_OwnerId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjectEmployees_Accounts_UserAccountId",
                table: "SalaryProjectEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjects_Accounts_EnterpriseAccountId",
                table: "SalaryProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_ReceiverId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_Accounts_SenderId",
                table: "Transfers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_UserAccount_EnterpriseId",
                table: "Account",
                newName: "IX_Account_UserAccount_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_OwnerId",
                table: "Account",
                newName: "IX_Account_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_EnterpriseId",
                table: "Account",
                newName: "IX_Account_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_BankId",
                table: "Account",
                newName: "IX_Account_BankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Banks_BankId",
                table: "Account",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Enterprises_EnterpriseId",
                table: "Account",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Enterprises_UserAccount_EnterpriseId",
                table: "Account",
                column: "UserAccount_EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Users_OwnerId",
                table: "Account",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryProjectEmployees_Account_UserAccountId",
                table: "SalaryProjectEmployees",
                column: "UserAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryProjects_Account_EnterpriseAccountId",
                table: "SalaryProjects",
                column: "EnterpriseAccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Account_ReceiverId",
                table: "Transfers",
                column: "ReceiverId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_Account_SenderId",
                table: "Transfers",
                column: "SenderId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
