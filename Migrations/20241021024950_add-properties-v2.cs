using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task_management.Migrations
{
    /// <inheritdoc />
    public partial class addpropertiesv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Tasks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Projects");
        }
    }
}
