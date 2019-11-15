using Microsoft.EntityFrameworkCore.Migrations;

namespace IPSAS.Persistence.Migrations
{
    public partial class UpdatedPayslip2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_MonthlyPayrolls_PayrollId",
                table: "Payslips");

            migrationBuilder.DropIndex(
                name: "IX_Payslips_PayrollId",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "PayrollId",
                table: "Payslips");

            migrationBuilder.AddColumn<int>(
                name: "MonthlyPayrollId",
                table: "Payslips",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_MonthlyPayrollId",
                table: "Payslips",
                column: "MonthlyPayrollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_MonthlyPayrolls_MonthlyPayrollId",
                table: "Payslips",
                column: "MonthlyPayrollId",
                principalTable: "MonthlyPayrolls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_MonthlyPayrolls_MonthlyPayrollId",
                table: "Payslips");

            migrationBuilder.DropIndex(
                name: "IX_Payslips_MonthlyPayrollId",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "MonthlyPayrollId",
                table: "Payslips");

            migrationBuilder.AddColumn<int>(
                name: "PayrollId",
                table: "Payslips",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_PayrollId",
                table: "Payslips",
                column: "PayrollId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_MonthlyPayrolls_PayrollId",
                table: "Payslips",
                column: "PayrollId",
                principalTable: "MonthlyPayrolls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
