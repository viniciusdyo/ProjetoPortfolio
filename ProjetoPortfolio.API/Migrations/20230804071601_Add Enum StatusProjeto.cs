using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoPortfolio.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEnumStatusProjeto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Projetos",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Status",
                table: "Projetos",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            
        }
    }
}
