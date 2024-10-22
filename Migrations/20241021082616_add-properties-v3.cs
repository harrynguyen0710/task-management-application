using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task_management.Migrations
{
    /// <inheritdoc />
    public partial class addpropertiesv3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "address",
                table: "AspNetUsers");
        }
    }
}
