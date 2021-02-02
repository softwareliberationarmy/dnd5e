using Microsoft.EntityFrameworkCore.Migrations;

namespace DnD_5e.Infrastructure.Migrations
{
    public partial class Add_Race_and_Class : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Class",
                table: "Character",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Race",
                table: "Character",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Race",
                table: "Character");
        }
    }
}
