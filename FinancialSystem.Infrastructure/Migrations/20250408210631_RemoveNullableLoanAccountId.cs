using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNullableLoanAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loans_Accounts_LoanAccountId",
                table: "Loans");

            migrationBuilder.AlterColumn<int>(
                name: "LoanAccountId",
                table: "Loans",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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

            migrationBuilder.AlterColumn<int>(
                name: "LoanAccountId",
                table: "Loans",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Loans_Accounts_LoanAccountId",
                table: "Loans",
                column: "LoanAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
