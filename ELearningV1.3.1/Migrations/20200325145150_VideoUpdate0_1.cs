using Microsoft.EntityFrameworkCore.Migrations;

namespace ELearningV1._3._1.Migrations
{
    public partial class VideoUpdate0_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ModuleContentId",
                table: "Videos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Videos_ModuleContentId",
                table: "Videos",
                column: "ModuleContentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Videos_ModuleContents_ModuleContentId",
                table: "Videos",
                column: "ModuleContentId",
                principalTable: "ModuleContents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Videos_ModuleContents_ModuleContentId",
                table: "Videos");

            migrationBuilder.DropIndex(
                name: "IX_Videos_ModuleContentId",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "ModuleContentId",
                table: "Videos");
        }
    }
}
