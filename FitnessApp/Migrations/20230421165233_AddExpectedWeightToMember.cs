using Microsoft.EntityFrameworkCore.Migrations;

namespace FitnessApp.Migrations
{
    public partial class AddExpectedWeightToMember : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BMIStatus",
                table: "Members",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExpectedWeight",
                table: "Members",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsMemberOverWeight",
                table: "Members",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BMIStatus",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ExpectedWeight",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "IsMemberOverWeight",
                table: "Members");
        }
    }
}
