using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAccountsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Banks_BankId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Enterprises_EnterpriseId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Users_OwnerId",
                table: "Accounts");

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
                newName: "UserAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_OwnerId",
                table: "UserAccounts",
                newName: "IX_UserAccounts_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_EnterpriseId",
                table: "UserAccounts",
                newName: "IX_UserAccounts_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_BankId",
                table: "UserAccounts",
                newName: "IX_UserAccounts_BankId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_UserAccounts_ReceiverId",
                table: "Transfers",
                column: "ReceiverId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transfers_UserAccounts_SenderId",
                table: "Transfers",
                column: "SenderId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Banks_BankId",
                table: "UserAccounts",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Enterprises_EnterpriseId",
                table: "UserAccounts",
                column: "EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Users_OwnerId",
                table: "UserAccounts",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_UserAccounts_ReceiverId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_Transfers_UserAccounts_SenderId",
                table: "Transfers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Banks_BankId",
                table: "UserAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Enterprises_EnterpriseId",
                table: "UserAccounts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Users_OwnerId",
                table: "UserAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts");

            migrationBuilder.RenameTable(
                name: "UserAccounts",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_OwnerId",
                table: "Accounts",
                newName: "IX_Accounts_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_EnterpriseId",
                table: "Accounts",
                newName: "IX_Accounts_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_BankId",
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
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Users_OwnerId",
                table: "Accounts",
                column: "OwnerId",
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
        }
    }
}
