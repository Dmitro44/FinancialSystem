using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AccountTPH : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjectEmployees_UserAccounts_UserAccountId",
                table: "SalaryProjectEmployees");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryProjects_EnterpriseAccounts_EnterpriseAccountId",
                table: "SalaryProjects");

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

            migrationBuilder.DropTable(
                name: "EnterpriseAccounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts");

            migrationBuilder.RenameTable(
                name: "UserAccounts",
                newName: "Account");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_OwnerId",
                table: "Account",
                newName: "IX_Account_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_EnterpriseId",
                table: "Account",
                newName: "IX_Account_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccounts_BankId",
                table: "Account",
                newName: "IX_Account_BankId");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "Account",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "AccountType",
                table: "Account",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "Discriminator",
                table: "Account",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserAccount_EnterpriseId",
                table: "Account",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Account",
                table: "Account",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserAccount_EnterpriseId",
                table: "Account",
                column: "UserAccount_EnterpriseId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropIndex(
                name: "IX_Account_UserAccount_EnterpriseId",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Account");

            migrationBuilder.DropColumn(
                name: "UserAccount_EnterpriseId",
                table: "Account");

            migrationBuilder.RenameTable(
                name: "Account",
                newName: "UserAccounts");

            migrationBuilder.RenameIndex(
                name: "IX_Account_OwnerId",
                table: "UserAccounts",
                newName: "IX_UserAccounts_OwnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_EnterpriseId",
                table: "UserAccounts",
                newName: "IX_UserAccounts_EnterpriseId");

            migrationBuilder.RenameIndex(
                name: "IX_Account_BankId",
                table: "UserAccounts",
                newName: "IX_UserAccounts_BankId");

            migrationBuilder.AlterColumn<int>(
                name: "OwnerId",
                table: "UserAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountType",
                table: "UserAccounts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccounts",
                table: "UserAccounts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EnterpriseAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BankId = table.Column<int>(type: "integer", nullable: false),
                    EnterpriseId = table.Column<int>(type: "integer", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false)
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
                name: "FK_SalaryProjectEmployees_UserAccounts_UserAccountId",
                table: "SalaryProjectEmployees",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryProjects_EnterpriseAccounts_EnterpriseAccountId",
                table: "SalaryProjects",
                column: "EnterpriseAccountId",
                principalTable: "EnterpriseAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
