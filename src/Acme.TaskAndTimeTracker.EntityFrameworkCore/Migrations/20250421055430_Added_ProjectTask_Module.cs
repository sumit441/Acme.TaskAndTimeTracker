using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Acme.TaskAndTimeTracker.Migrations
{
    /// <inheritdoc />
    public partial class Added_ProjectTask_Module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId1",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ProjectId1",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProjectId1",
                table: "ProjectTasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssignedUserId",
                table: "ProjectTasks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "AssignedUserId",
                table: "ProjectTasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId1",
                table: "ProjectTasks",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectId1",
                table: "ProjectTasks",
                column: "ProjectId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_Projects_ProjectId1",
                table: "ProjectTasks",
                column: "ProjectId1",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
