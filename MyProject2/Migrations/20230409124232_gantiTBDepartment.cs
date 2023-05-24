using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyProject2.Migrations
{
    /// <inheritdoc />
    public partial class gantiTBDepartment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_TB_Department_DepartmentID",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TB_Department",
                table: "TB_Department");

            migrationBuilder.RenameTable(
                name: "TB_Department",
                newName: "Department");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Department",
                table: "Department",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Department_DepartmentID",
                table: "Employee",
                column: "DepartmentID",
                principalTable: "Department",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Department_DepartmentID",
                table: "Employee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Department",
                table: "Department");

            migrationBuilder.RenameTable(
                name: "Department",
                newName: "TB_Department");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TB_Department",
                table: "TB_Department",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_TB_Department_DepartmentID",
                table: "Employee",
                column: "DepartmentID",
                principalTable: "TB_Department",
                principalColumn: "ID");
        }
    }
}
