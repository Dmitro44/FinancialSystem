using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DestinationInstallmentAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DestinationAccountId",
                table: "Installments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InstallmentAccountId",
                table: "Installments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Installments_InstallmentAccountId",
                table: "Installments",
                column: "InstallmentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Installments_Accounts_InstallmentAccountId",
                table: "Installments",
                column: "InstallmentAccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Installments_Accounts_InstallmentAccountId",
                table: "Installments");

            migrationBuilder.DropIndex(
                name: "IX_Installments_InstallmentAccountId",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "DestinationAccountId",
                table: "Installments");

            migrationBuilder.DropColumn(
                name: "InstallmentAccountId",
                table: "Installments");
        }
    }
}
