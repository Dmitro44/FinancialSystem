using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFieldsToSalaryProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "SalaryProjects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "SalaryProjects",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryProjects_BankId",
                table: "SalaryProjects",
                column: "BankId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryProjects_Banks_BankId",
                table: "SalaryProjects",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjects_Banks_BankId",
                table: "SalaryProjects");

            migrationBuilder.DropIndex(
                name: "IX_SalaryProjects_BankId",
                table: "SalaryProjects");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "SalaryProjects");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "SalaryProjects");
        }
    }
}
