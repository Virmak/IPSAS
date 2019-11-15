using Microsoft.EntityFrameworkCore.Migrations;

namespace IPSAS.Persistence.Migrations
{
    public partial class AddedPaymentReference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_Payment_PaymentId",
                table: "Payslips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.AddColumn<string>(
                name: "Reference",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_Payments_PaymentId",
                table: "Payslips",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_Payments_PaymentId",
                table: "Payslips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Reference",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_Payment_PaymentId",
                table: "Payslips",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
