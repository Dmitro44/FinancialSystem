using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AccountTPHFixEmployer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Enterprises_UserAccount_EnterpriseId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "UserAccount_EnterpriseId",
                table: "Accounts",
                newName: "EmployerEnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_UserAccount_EnterpriseId",
                table: "Accounts",
                newName: "IX_Accounts_EmployerEnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Enterprises_EmployerEnterpriseId",
                table: "Accounts",
                column: "EmployerEnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Enterprises_EmployerEnterpriseId",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "EmployerEnterpriseId",
                table: "Accounts",
                newName: "UserAccount_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_EmployerEnterpriseId",
                table: "Accounts",
                newName: "IX_Accounts_UserAccount_EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Enterprises_UserAccount_EnterpriseId",
                table: "Accounts",
                column: "UserAccount_EnterpriseId",
                principalTable: "Enterprises",
                principalColumn: "Id");
        }
    }
}
