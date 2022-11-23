using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GAMMAFEST.Migrations
{
    /// <inheritdoc />
    public partial class dataMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entrada_RegistroAsistencia_RegistroAsistenciaId",
                table: "Entrada");

            migrationBuilder.DropIndex(
                name: "IX_Entrada_RegistroAsistenciaId",
                table: "Entrada");

            migrationBuilder.DropColumn(
                name: "RegistroAsistenciaId",
                table: "Entrada");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroAsistencia_EntradaId",
                table: "RegistroAsistencia",
                column: "EntradaId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistroAsistencia_Entrada_EntradaId",
                table: "RegistroAsistencia",
                column: "EntradaId",
                principalTable: "Entrada",
                principalColumn: "EntradaId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistroAsistencia_Entrada_EntradaId",
                table: "RegistroAsistencia");

            migrationBuilder.DropIndex(
                name: "IX_RegistroAsistencia_EntradaId",
                table: "RegistroAsistencia");

            migrationBuilder.AddColumn<int>(
                name: "RegistroAsistenciaId",
                table: "Entrada",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_RegistroAsistenciaId",
                table: "Entrada",
                column: "RegistroAsistenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entrada_RegistroAsistencia_RegistroAsistenciaId",
                table: "Entrada",
                column: "RegistroAsistenciaId",
                principalTable: "RegistroAsistencia",
                principalColumn: "Id");
        }
    }
}
