using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consultorio_Seguros.Migrations
{
    /// <inheritdoc />
    public partial class cambio3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Seguros",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ramo",
                table: "Seguros",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Asegurados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Asegurados",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Seguros");

            migrationBuilder.DropColumn(
                name: "Ramo",
                table: "Seguros");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Asegurados");

            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Asegurados");
        }
    }
}
