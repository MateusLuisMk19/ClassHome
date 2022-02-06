using Microsoft.EntityFrameworkCore.Migrations;

namespace ClassHome.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TurmaUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TurmaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TurmaUser", x => new { x.UserId, x.TurmaId });
                    table.ForeignKey(
                        name: "FK_TurmaUser_Turma_TurmaId",
                        column: x => x.TurmaId,
                        principalTable: "Turma",
                        principalColumn: "TurmaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TurmaUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TurmaUser_TurmaId",
                table: "TurmaUser",
                column: "TurmaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TurmaUser");
        }
    }
}
