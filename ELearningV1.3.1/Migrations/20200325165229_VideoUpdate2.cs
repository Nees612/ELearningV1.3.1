using Microsoft.EntityFrameworkCore.Migrations;

namespace ELearningV1._3._1.Migrations
{
    public partial class VideoUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TutorialUrl",
                table: "ModuleContents");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TutorialUrl",
                table: "ModuleContents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
