using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.TaskAndTimeTracker.Migrations
{
    /// <inheritdoc />
    public partial class FixProjectTaskNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AssignedUserId",
                table: "ProjectTasks",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_AbpUsers_AssignedUserId",
                table: "ProjectTasks",
                column: "AssignedUserId",
                principalTable: "AbpUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_AbpUsers_AssignedUserId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_AssignedUserId",
                table: "ProjectTasks");
        }
    }
}
