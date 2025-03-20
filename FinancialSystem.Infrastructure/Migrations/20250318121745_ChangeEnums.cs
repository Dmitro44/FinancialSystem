using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "Installments");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Installments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Installments");

            migrationBuilder.AddColumn<decimal>(
                name: "InterestRate",
                table: "Installments",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
