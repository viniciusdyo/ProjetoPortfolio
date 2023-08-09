using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoPortfolio.API.Migrations
{
    /// <inheritdoc />
    public partial class Conteudo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriaConteudo",
                columns: table => new
                {
                    CategoriaConteudoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    ConteudoModelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaConteudo", x => x.CategoriaConteudoId);
                });

            migrationBuilder.CreateTable(
                name: "Conteudo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Conteudo = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CategoriaConteudoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoriaConteudoModelCategoriaConteudoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conteudo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conteudo_CategoriaConteudo_CategoriaConteudoModelCategoriaConteudoId",
                        column: x => x.CategoriaConteudoModelCategoriaConteudoId,
                        principalTable: "CategoriaConteudo",
                        principalColumn: "CategoriaConteudoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaConteudo_ConteudoModelId",
                table: "CategoriaConteudo",
                column: "ConteudoModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Conteudo_CategoriaConteudoModelCategoriaConteudoId",
                table: "Conteudo",
                column: "CategoriaConteudoModelCategoriaConteudoId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaConteudo_Conteudo_ConteudoModelId",
                table: "CategoriaConteudo",
                column: "ConteudoModelId",
                principalTable: "Conteudo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaConteudo_Conteudo_ConteudoModelId",
                table: "CategoriaConteudo");

            migrationBuilder.DropTable(
                name: "Conteudo");

            migrationBuilder.DropTable(
                name: "CategoriaConteudo");
        }
    }
}
