using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ClassHome.Migrations
{
    public partial class versao0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matricula_Aluno_AlunoId",
                table: "Matricula");

            migrationBuilder.DropForeignKey(
                name: "FK_Matricula_Disciplina_DisciplinaId",
                table: "Matricula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matricula",
                table: "Matricula");

            migrationBuilder.RenameTable(
                name: "Matricula",
                newName: "Matriculas");

            migrationBuilder.RenameIndex(
                name: "IX_Matricula_DisciplinaId",
                table: "Matriculas",
                newName: "IX_Matriculas_DisciplinaId");

            migrationBuilder.RenameIndex(
                name: "IX_Matricula_AlunoId",
                table: "Matriculas",
                newName: "IX_Matriculas_AlunoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matriculas",
                table: "Matriculas",
                column: "MatriculaId");

            migrationBuilder.CreateTable(
                name: "Publicacao",
                columns: table => new
                {
                    PublicacaoId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Texto = table.Column<string>(type: "TEXT", nullable: true),
                    DataPublicacao = table.Column<DateTime>(type: "TEXT", nullable: true, defaultValueSql: "datetime('now')"),
                    AlunoId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisciplinaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Publicacao", x => x.PublicacaoId);
                    table.ForeignKey(
                        name: "FK_Publicacao_Aluno_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Aluno",
                        principalColumn: "AlunoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Publicacao_Disciplina_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplina",
                        principalColumn: "DisciplinaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comentario",
                columns: table => new
                {
                    ComentarioId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Texto = table.Column<string>(type: "TEXT", nullable: true),
                    AlunoId = table.Column<int>(type: "INTEGER", nullable: false),
                    PublicacaoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentario", x => x.ComentarioId);
                    table.ForeignKey(
                        name: "FK_Comentario_Aluno_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Aluno",
                        principalColumn: "AlunoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentario_Publicacao_PublicacaoId",
                        column: x => x.PublicacaoId,
                        principalTable: "Publicacao",
                        principalColumn: "PublicacaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_AlunoId",
                table: "Comentario",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_PublicacaoId",
                table: "Comentario",
                column: "PublicacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacao_AlunoId",
                table: "Publicacao",
                column: "AlunoId");

            migrationBuilder.CreateIndex(
                name: "IX_Publicacao_DisciplinaId",
                table: "Publicacao",
                column: "DisciplinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Aluno_AlunoId",
                table: "Matriculas",
                column: "AlunoId",
                principalTable: "Aluno",
                principalColumn: "AlunoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Disciplina_DisciplinaId",
                table: "Matriculas",
                column: "DisciplinaId",
                principalTable: "Disciplina",
                principalColumn: "DisciplinaId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Aluno_AlunoId",
                table: "Matriculas");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Disciplina_DisciplinaId",
                table: "Matriculas");

            migrationBuilder.DropTable(
                name: "Comentario");

            migrationBuilder.DropTable(
                name: "Publicacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matriculas",
                table: "Matriculas");

            migrationBuilder.RenameTable(
                name: "Matriculas",
                newName: "Matricula");

            migrationBuilder.RenameIndex(
                name: "IX_Matriculas_DisciplinaId",
                table: "Matricula",
                newName: "IX_Matricula_DisciplinaId");

            migrationBuilder.RenameIndex(
                name: "IX_Matriculas_AlunoId",
                table: "Matricula",
                newName: "IX_Matricula_AlunoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matricula",
                table: "Matricula",
                column: "MatriculaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matricula_Aluno_AlunoId",
                table: "Matricula",
                column: "AlunoId",
                principalTable: "Aluno",
                principalColumn: "AlunoId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matricula_Disciplina_DisciplinaId",
                table: "Matricula",
                column: "DisciplinaId",
                principalTable: "Disciplina",
                principalColumn: "DisciplinaId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
