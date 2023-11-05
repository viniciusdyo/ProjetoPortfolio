using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoPortfolio.API.Migrations
{
    /// <inheritdoc />
    public partial class EmailConfigMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habilidades_PerfilModel_Id",
                table: "Habilidades");

            migrationBuilder.DropForeignKey(
                name: "FK_RedeModel_PerfilModel_Id",
                table: "RedeModel");

            migrationBuilder.DropForeignKey(
                name: "FK_RedeModel_Pessoas_Id",
                table: "RedeModel");


            migrationBuilder.DropPrimaryKey(
                name: "PK_RedeModel",
                table: "RedeModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PerfilModel",
                table: "PerfilModel");

            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "Habilidades");

            migrationBuilder.RenameTable(
                name: "RedeModel",
                newName: "Redes");

            migrationBuilder.RenameTable(
                name: "PerfilModel",
                newName: "Perfis");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Redes",
                table: "Redes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Perfis",
                table: "Perfis",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "EmailConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Servidor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Porta = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfig", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Habilidades_Perfis_Id",
                table: "Habilidades",
                column: "Id",
                principalTable: "Perfis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Redes_Perfis_Id",
                table: "Redes",
                column: "Id",
                principalTable: "Perfis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Redes_Pessoas_Id",
                table: "Redes",
                column: "Id",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Habilidades_Perfis_Id",
                table: "Habilidades");

            migrationBuilder.DropForeignKey(
                name: "FK_Redes_Perfis_Id",
                table: "Redes");

            migrationBuilder.DropForeignKey(
                name: "FK_Redes_Pessoas_Id",
                table: "Redes");

            migrationBuilder.DropTable(
                name: "EmailConfig");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Redes",
                table: "Redes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Perfis",
                table: "Perfis");

            migrationBuilder.RenameTable(
                name: "Redes",
                newName: "RedeModel");

            migrationBuilder.RenameTable(
                name: "Perfis",
                newName: "PerfilModel");

            migrationBuilder.AddColumn<Guid>(
                name: "PerfilId",
                table: "Habilidades",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_RedeModel",
                table: "RedeModel",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PerfilModel",
                table: "PerfilModel",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Habilidades_PerfilId",
                table: "Habilidades",
                column: "PerfilId");

            migrationBuilder.AddForeignKey(
                name: "FK_Habilidades_PerfilModel_Id",
                table: "Habilidades",
                column: "Id",
                principalTable: "PerfilModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RedeModel_PerfilModel_Id",
                table: "RedeModel",
                column: "Id",
                principalTable: "PerfilModel",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RedeModel_Pessoas_Id",
                table: "RedeModel",
                column: "Id",
                principalTable: "Pessoas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
