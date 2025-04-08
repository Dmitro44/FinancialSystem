using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LoanAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LoanAccountId",
                table: "Loans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Loans_LoanAccountId",
                table: "Loans",
                column: "LoanAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Accounts_LoanAccountId",
                table: "Loans",
                column: "LoanAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Accounts_LoanAccountId",
                table: "Loans");

            migrationBuilder.DropIndex(
                name: "IX_Loans_LoanAccountId",
                table: "Loans");

            migrationBuilder.DropColumn(
                name: "LoanAccountId",
                table: "Loans");
        }
    }
}
