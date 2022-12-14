using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GAMMAFEST.Migrations
{
    /// <inheritdoc />
    public partial class dataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPromotor",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Contrasenia = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cifrado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    tipoUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPromotor", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "Evento",
                columns: table => new
                {
                    EventoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaInicioEvento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreEvento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Protocolos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    AforoMaximo = table.Column<float>(type: "real", nullable: false),
                    PrecioEntradaUnit = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Evento", x => x.EventoId);
                    table.ForeignKey(
                        name: "FK_Evento_UserPromotor_IdUser",
                        column: x => x.IdUser,
                        principalTable: "UserPromotor",
                        principalColumn: "IdUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Entrada",
                columns: table => new
                {
                    EntradaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CantidadEntradas = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<int>(type: "int", nullable: false),
                    PrecioTotal = table.Column<int>(type: "int", nullable: false),
                    TextoQR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdCantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entrada", x => x.EntradaId);
                    table.ForeignKey(
                        name: "FK_Entrada_Evento_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Evento",
                        principalColumn: "EventoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegistroAsistencia",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreImagen = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntradaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistroAsistencia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistroAsistencia_Entrada_EntradaId",
                        column: x => x.EntradaId,
                        principalTable: "Entrada",
                        principalColumn: "EntradaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entrada_EventoId",
                table: "Entrada",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Evento_IdUser",
                table: "Evento",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_RegistroAsistencia_EntradaId",
                table: "RegistroAsistencia",
                column: "EntradaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistroAsistencia");

            migrationBuilder.DropTable(
                name: "Entrada");

            migrationBuilder.DropTable(
                name: "Evento");

            migrationBuilder.DropTable(
                name: "UserPromotor");
        }
    }
}
