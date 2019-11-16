using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IPSAS.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthlyPayrolls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<int>(nullable: false),
                    AcademicYear = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyPayrolls", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bank = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentType = table.Column<int>(nullable: false),
                    Reference = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Remunerations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Grade = table.Column<int>(nullable: false),
                    HourlyRate = table.Column<double>(nullable: false),
                    Salary = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remunerations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CIN = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<int>(nullable: false),
                    HomeInstitution = table.Column<string>(nullable: true),
                    Speciality = table.Column<string>(nullable: true),
                    Grade = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ContractType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PayrollRecords",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherId = table.Column<int>(nullable: false),
                    PayrollId = table.Column<int>(nullable: false),
                    Rate = table.Column<double>(nullable: false),
                    HoursCount = table.Column<int>(nullable: false),
                    Retenu = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PayrollRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PayrollRecords_MonthlyPayrolls_PayrollId",
                        column: x => x.PayrollId,
                        principalTable: "MonthlyPayrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PayrollRecords_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payslips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthlyPayrollId = table.Column<int>(nullable: true),
                    TeacherId = table.Column<int>(nullable: true),
                    PaymentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payslips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payslips_MonthlyPayrolls_MonthlyPayrollId",
                        column: x => x.MonthlyPayrollId,
                        principalTable: "MonthlyPayrolls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payslips_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
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
                name: "IX_PayrollRecords_PayrollId",
                table: "PayrollRecords",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_PayrollRecords_TeacherId_PayrollId",
                table: "PayrollRecords",
                columns: new[] { "TeacherId", "PayrollId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_MonthlyPayrollId",
                table: "Payslips",
                column: "MonthlyPayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_PaymentId",
                table: "Payslips",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_TeacherId",
                table: "Payslips",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CIN",
                table: "Teachers",
                column: "CIN",
                unique: true,
                filter: "[CIN] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PayrollRecords");

            migrationBuilder.DropTable(
                name: "Payslips");

            migrationBuilder.DropTable(
                name: "Remunerations");

            migrationBuilder.DropTable(
                name: "MonthlyPayrolls");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
