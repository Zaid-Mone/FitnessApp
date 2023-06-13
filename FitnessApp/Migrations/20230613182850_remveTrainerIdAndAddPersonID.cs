using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessApp.Migrations
{
    public partial class remveTrainerIdAndAddPersonID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Trainers_TrainerId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Nutritions_Trainers_TrainerId",
                table: "Nutritions");

            migrationBuilder.DropIndex(
                name: "IX_Nutritions_TrainerId",
                table: "Nutritions");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_TrainerId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Nutritions");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "Nutritions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "Exercises",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nutritions_PersonId",
                table: "Nutritions",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_PersonId",
                table: "Exercises",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Trainers_PersonId",
                table: "Exercises",
                column: "PersonId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Nutritions_Trainers_PersonId",
                table: "Nutritions",
                column: "PersonId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Trainers_PersonId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Nutritions_Trainers_PersonId",
                table: "Nutritions");

            migrationBuilder.DropIndex(
                name: "IX_Nutritions_PersonId",
                table: "Nutritions");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_PersonId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Nutritions");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "TrainerId",
                table: "Nutritions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrainerId",
                table: "Exercises",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Nutritions_TrainerId",
                table: "Nutritions",
                column: "TrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_TrainerId",
                table: "Exercises",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Trainers_TrainerId",
                table: "Exercises",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Nutritions_Trainers_TrainerId",
                table: "Nutritions",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
