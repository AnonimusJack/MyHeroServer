using Microsoft.EntityFrameworkCore.Migrations;

namespace MyHeroServer.Migrations
{
    public partial class AddedTrainingValueForHero : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InTraining",
                table: "Heroes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InTraining",
                table: "Heroes");
        }
    }
}
