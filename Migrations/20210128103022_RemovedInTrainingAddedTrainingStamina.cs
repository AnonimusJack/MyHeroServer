using Microsoft.EntityFrameworkCore.Migrations;

namespace MyHeroServer.Migrations
{
    public partial class RemovedInTrainingAddedTrainingStamina : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InTraining",
                table: "Heroes");

            migrationBuilder.AddColumn<int>(
                name: "TrainingStamina",
                table: "Heroes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrainingStamina",
                table: "Heroes");

            migrationBuilder.AddColumn<bool>(
                name: "InTraining",
                table: "Heroes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
