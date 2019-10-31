using Microsoft.EntityFrameworkCore.Migrations;

namespace IPSAS.Persistence.Migrations
{
    public partial class IntitalCreate : Migration
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
                name: "MonthlyPayrolls");

            migrationBuilder.DropTable(
                name: "Teachers");
        }
    }
}
