using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class StudentUpdateMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentCode",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentId",
                table: "Students",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentCode",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
