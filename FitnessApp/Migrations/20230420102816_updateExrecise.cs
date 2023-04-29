using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessApp.Migrations
{
    public partial class updateExrecise : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Members_MemberId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_TrainersMembers_Nutritions_NutritionId",
                table: "TrainersMembers");

            migrationBuilder.DropIndex(
                name: "IX_TrainersMembers_NutritionId",
                table: "TrainersMembers");

            migrationBuilder.DropColumn(
                name: "NutritionId",
                table: "TrainersMembers");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Exercises",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Exercises",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExerciseDuration",
                table: "Exercises",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExerciseFrom",
                table: "Exercises",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ExerciseTO",
                table: "Exercises",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "ExerciseTimeFormat",
                table: "Exercises",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Members_MemberId",
                table: "Exercises",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Members_MemberId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExerciseFrom",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExerciseTO",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExerciseTimeFormat",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "NutritionId",
                table: "TrainersMembers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "MemberId",
                table: "Exercises",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "ExerciseDuration",
                table: "Exercises",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TrainersMembers_NutritionId",
                table: "TrainersMembers",
                column: "NutritionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Members_MemberId",
                table: "Exercises",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TrainersMembers_Nutritions_NutritionId",
                table: "TrainersMembers",
                column: "NutritionId",
                principalTable: "Nutritions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
