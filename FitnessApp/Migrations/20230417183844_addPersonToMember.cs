using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessApp.Migrations
{
    public partial class addPersonToMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "Members",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_PersonId",
                table: "Members",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_PersonId",
                table: "Members",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_PersonId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_PersonId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Members");
        }
    }
}
