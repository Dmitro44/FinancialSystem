using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnterpriseAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_SalaryProjects_SalaryProjectId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjects_Accounts_EnterpriseAccountId",
                table: "SalaryProjects");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_SalaryProjectId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SalaryProjects");

            migrationBuilder.DropColumn(
                name: "SalaryProjectId",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "SalaryProjects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "EnterpriseAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnterpriseId = table.Column<int>(type: "integer", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    BankId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterpriseAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterpriseAccounts_Banks_BankId",
                        column: x => x.BankId,
                        principalTable: "Banks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EnterpriseAccounts_Enterprises_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseAccounts_BankId",
                table: "EnterpriseAccounts",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_EnterpriseAccounts_EnterpriseId",
                table: "EnterpriseAccounts",
                column: "EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryProjects_EnterpriseAccounts_EnterpriseAccountId",
                table: "SalaryProjects",
                column: "EnterpriseAccountId",
                principalTable: "EnterpriseAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjects_EnterpriseAccounts_EnterpriseAccountId",
                table: "SalaryProjects");

            migrationBuilder.DropTable(
                name: "EnterpriseAccounts");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "SalaryProjects");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SalaryProjects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SalaryProjectId",
                table: "Accounts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_SalaryProjectId",
                table: "Accounts",
                column: "SalaryProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_SalaryProjects_SalaryProjectId",
                table: "Accounts",
                column: "SalaryProjectId",
                principalTable: "SalaryProjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryProjects_Accounts_EnterpriseAccountId",
                table: "SalaryProjects",
                column: "EnterpriseAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
