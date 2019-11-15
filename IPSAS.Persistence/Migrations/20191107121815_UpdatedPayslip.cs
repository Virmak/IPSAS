using Microsoft.EntityFrameworkCore.Migrations;

namespace IPSAS.Persistence.Migrations
{
    public partial class UpdatedPayslip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcademicYear",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "Month",
                table: "Payslips");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcademicYear",
                table: "Payslips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Month",
                table: "Payslips",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
