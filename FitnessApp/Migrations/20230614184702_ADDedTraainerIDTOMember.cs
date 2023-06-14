using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessApp.Migrations
{
    public partial class ADDedTraainerIDTOMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrainerId",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_TrainerId",
                table: "Members",
                column: "TrainerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Trainers_TrainerId",
                table: "Members",
                column: "TrainerId",
                principalTable: "Trainers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Trainers_TrainerId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_TrainerId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "TrainerId",
                table: "Members");
        }
    }
}
