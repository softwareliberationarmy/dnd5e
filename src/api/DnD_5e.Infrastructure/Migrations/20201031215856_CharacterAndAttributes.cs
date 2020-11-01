using Microsoft.EntityFrameworkCore.Migrations;

namespace DnD_5e.Infrastructure.Migrations
{
    public partial class CharacterAndAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Strength = table.Column<int>(nullable: false),
                    Dexterity = table.Column<int>(nullable: false),
                    Constitution = table.Column<int>(nullable: false),
                    Intelligence = table.Column<int>(nullable: false),
                    Wisdom = table.Column<int>(nullable: false),
                    Charisma = table.Column<int>(nullable: false),
                    StrengthSaveProficiency = table.Column<bool>(nullable: false),
                    DexteritySaveProficiency = table.Column<bool>(nullable: false),
                    ConstitutionSaveProficiency = table.Column<bool>(nullable: false),
                    IntelligenceSaveProficiency = table.Column<bool>(nullable: false),
                    WisdomSaveProficiency = table.Column<bool>(nullable: false),
                    CharismaSaveProficiency = table.Column<bool>(nullable: false),
                    ExperiencePoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillProficiencyEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    CharacterId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillProficiencyEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillProficiencyEntity_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SkillProficiencyEntity_CharacterId",
                table: "SkillProficiencyEntity",
                column: "CharacterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SkillProficiencyEntity");

            migrationBuilder.DropTable(
                name: "Characters");
        }
    }
}
