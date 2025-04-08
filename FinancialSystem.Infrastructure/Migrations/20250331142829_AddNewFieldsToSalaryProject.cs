using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNewFieldsToSalaryProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "UserAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserAccountId",
                table: "SalaryProjectEmployees",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryProjectEmployees_UserAccountId",
                table: "SalaryProjectEmployees",
                column: "UserAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryProjectEmployees_UserAccounts_UserAccountId",
                table: "SalaryProjectEmployees",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjectEmployees_UserAccounts_UserAccountId",
                table: "SalaryProjectEmployees");

            migrationBuilder.DropIndex(
                name: "IX_SalaryProjectEmployees_UserAccountId",
                table: "SalaryProjectEmployees");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "UserAccountId",
                table: "SalaryProjectEmployees");
        }
    }
}
