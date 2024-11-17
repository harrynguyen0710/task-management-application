using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task_management.Migrations
{
    /// <inheritdoc />
    public partial class updateTaskRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_userId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_userId",
                table: "Tasks");

            migrationBuilder.AddColumn<int>(
                name: "projectId",
                table: "Tasks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_projectId_userId",
                table: "Tasks",
                columns: new[] { "projectId", "userId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_ProjectAssignments_projectId_userId",
                table: "Tasks",
                columns: new[] { "projectId", "userId" },
                principalTable: "ProjectAssignments",
                principalColumns: new[] { "projectId", "userId" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_ProjectAssignments_projectId_userId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_projectId_userId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "projectId",
                table: "Tasks");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_userId",
                table: "Tasks",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_userId",
                table: "Tasks",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
