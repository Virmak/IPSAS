using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IPSAS.Persistence.Migrations
{
    public partial class AddedPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "PayementDate",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "PayementDetails",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "PayementType",
                table: "Payslips");

            migrationBuilder.AddColumn<int>(
                name: "PaymentId",
                table: "Payslips",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bank = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payslips_PaymentId",
                table: "Payslips",
                column: "PaymentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payslips_Payment_PaymentId",
                table: "Payslips",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payslips_Payment_PaymentId",
                table: "Payslips");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payslips_PaymentId",
                table: "Payslips");

            migrationBuilder.DropColumn(
                name: "PaymentId",
                table: "Payslips");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "Payslips",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PayementDate",
                table: "Payslips",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PayementDetails",
                table: "Payslips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PayementType",
                table: "Payslips",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
