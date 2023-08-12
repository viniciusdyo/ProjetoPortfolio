using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoPortfolio.API.Migrations
{
    /// <inheritdoc />
    public partial class PessoaRefatorada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HabilidadeId",
                table: "Pessoas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HabilidadeId",
                table: "Pessoas",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
