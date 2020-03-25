using Microsoft.EntityFrameworkCore.Migrations;

namespace ELearningV1._3._1.Migrations
{
    public partial class VideoUpdate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "YoutubeId",
                table: "Videos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YoutubeId",
                table: "Videos");
        }
    }
}
