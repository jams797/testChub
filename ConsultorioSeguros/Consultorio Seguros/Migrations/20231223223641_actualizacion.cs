using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Consultorio_Seguros.Migrations
{
    /// <inheritdoc />
    public partial class actualizacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asegurados_Seguros_SeguroId",
                table: "Asegurados");

            migrationBuilder.DropIndex(
                name: "IX_Asegurados_SeguroId",
                table: "Asegurados");

            migrationBuilder.DropColumn(
                name: "SeguroId",
                table: "Asegurados");

            migrationBuilder.CreateTable(
                name: "AseguradosModelSegurosModel",
                columns: table => new
                {
                    AseguradosId = table.Column<int>(type: "int", nullable: false),
                    SegurosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AseguradosModelSegurosModel", x => new { x.AseguradosId, x.SegurosId });
                    table.ForeignKey(
                        name: "FK_AseguradosModelSegurosModel_Asegurados_AseguradosId",
                        column: x => x.AseguradosId,
                        principalTable: "Asegurados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AseguradosModelSegurosModel_Seguros_SegurosId",
                        column: x => x.SegurosId,
                        principalTable: "Seguros",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AseguradosModelSegurosModel_SegurosId",
                table: "AseguradosModelSegurosModel",
                column: "SegurosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AseguradosModelSegurosModel");

            migrationBuilder.AddColumn<int>(
                name: "SeguroId",
                table: "Asegurados",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Asegurados_SeguroId",
                table: "Asegurados",
                column: "SeguroId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asegurados_Seguros_SeguroId",
                table: "Asegurados",
                column: "SeguroId",
                principalTable: "Seguros",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
