using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinancialSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSalaryProjectEmployeeAndUserEnterprise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SalaryProjectEmployees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SalaryProjectId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaryProjectEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalaryProjectEmployees_SalaryProjects_SalaryProjectId",
                        column: x => x.SalaryProjectId,
                        principalTable: "SalaryProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SalaryProjectEmployees_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SalaryProjectEmployees_SalaryProjectId",
                table: "SalaryProjectEmployees",
                column: "SalaryProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SalaryProjectEmployees_UserId",
                table: "SalaryProjectEmployees",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SalaryProjectEmployees");
        }
    }
}
