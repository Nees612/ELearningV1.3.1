using Microsoft.EntityFrameworkCore.Migrations;

namespace ELearningV1._3._1.Migrations
{
    public partial class assigmentsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ModuleId",
                table: "Assigments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assigments_ModuleId",
                table: "Assigments",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assigments_Modules_ModuleId",
                table: "Assigments",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assigments_Modules_ModuleId",
                table: "Assigments");

            migrationBuilder.DropIndex(
                name: "IX_Assigments_ModuleId",
                table: "Assigments");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Assigments");
        }
    }
}
