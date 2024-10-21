using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task_management.Migrations
{
    /// <inheritdoc />
    public partial class addpropertiesv1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "dueDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "startDate",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "target",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "fullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dueDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "startDate",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "target",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "fullName",
                table: "AspNetUsers");
        }
    }
}
