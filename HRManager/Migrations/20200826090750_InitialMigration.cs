using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HRManager.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendances",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeID = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true),
                    Department = table.Column<int>(nullable: true),
                    WorkingDate = table.Column<DateTime>(nullable: false),
                    CheckType = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Credentials",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credentials", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Role = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    MiddleName = table.Column<string>(maxLength: 50, nullable: true),
                    Dept = table.Column<int>(nullable: false),
                    Gender = table.Column<string>(nullable: false),
                    DOB = table.Column<string>(nullable: false),
                    HomeAddress = table.Column<string>(nullable: false),
                    TellNo = table.Column<string>(nullable: true),
                    MobileNo = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Credentials",
                columns: new[] { "ID", "EmailAddress", "Password" },
                values: new object[] { 1, "yashas348@gmail.com", "289d57d1da73f2ed77a0f93db9b1338021638d1414148ee05b7cb47383733020" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "DOB", "Dept", "EmailAddress", "FirstName", "Gender", "HomeAddress", "ImagePath", "LastName", "MiddleName", "MobileNo", "Role", "TellNo" },
                values: new object[] { 1, "03/11/1997", 3, "yashas348@gmail.com", "Yashas", "Male", "Bangalore, Karnataka, India", null, "Gowda", null, "9108831063", true, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances");

            migrationBuilder.DropTable(
                name: "Credentials");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
