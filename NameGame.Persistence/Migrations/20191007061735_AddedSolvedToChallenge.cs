using Microsoft.EntityFrameworkCore.Migrations;

namespace NameGame.Persistence.Migrations {
    public partial class AddedSolvedToChallenge : Migration {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<bool>(
                name: "Solved",
                table: "Challenges",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "Solved",
                table: "Challenges");
        }
    }
}
