using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IPSAS.Persistence.Migrations
{
    public partial class AddedPayslip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payslips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<int>(nullable: false),
                    AcademicYear = table.Column<int>(nullable: false),
                    TeacherId = table.Column<int>(nullable: true),
                    PayrollId = table.Column<int>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    PayementType = table.Column<int>(nullable: false),
                    PayementDate = table.Column<DateTime>(nullable: false),
                    PayementDetails = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payslips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payslips_MonthlyPayrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "MonthlyPayrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payslips_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_PayrollId",
                table: "Payslips",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_TeacherId",
                table: "Payslips",
                column: "TeacherId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payslips");
        }
    }
}
