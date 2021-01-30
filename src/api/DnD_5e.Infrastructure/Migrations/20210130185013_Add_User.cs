using Microsoft.EntityFrameworkCore.Migrations;

namespace DnD_5e.Infrastructure.Migrations
{
    public partial class Add_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Character",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Character",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Character_OwnerId",
                table: "Character",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Character_User_OwnerId",
                table: "Character",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Character_User_OwnerId",
                table: "Character");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropIndex(
                name: "IX_Character_OwnerId",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Character");
        }
    }
}
