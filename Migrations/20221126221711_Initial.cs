using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WinFormsApp1.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CajaDeAhorro",
                columns: table => new
                {
                    IdCajaAhorro = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cbu = table.Column<int>(type: "int", nullable: false),
                    Saldo = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajaDeAhorro", x => x.IdCajaAhorro);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Dni = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "varchar(50)", nullable: false),
                    Apellido = table.Column<string>(type: "varchar(50)", nullable: false),
                    Mail = table.Column<string>(type: "varchar(50)", nullable: false),
                    Clave = table.Column<string>(type: "varchar(50)", nullable: false),
                    IntentosFallidos = table.Column<int>(type: "int", nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    IsBloqueado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.IdUsuario);
                });

            migrationBuilder.CreateTable(
                name: "Movimiento",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detalle = table.Column<string>(type: "varchar(50)", nullable: false),
                    Monto = table.Column<float>(type: "real", nullable: false),
                    Fecha = table.Column<DateTime>(type: "date", nullable: false),
                    IdCajaAhorro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movimiento", x => x.IdMovimiento);
                    table.ForeignKey(
                        name: "FK_Movimiento_CajaDeAhorro_IdCajaAhorro",
                        column: x => x.IdCajaAhorro,
                        principalTable: "CajaDeAhorro",
                        principalColumn: "IdCajaAhorro",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CajasUsuario",
                columns: table => new
                {
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    IdCajaAhorro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajasUsuario", x => new { x.IdUsuario, x.IdCajaAhorro });
                    table.ForeignKey(
                        name: "FK_CajasUsuario_CajaDeAhorro_IdCajaAhorro",
                        column: x => x.IdCajaAhorro,
                        principalTable: "CajaDeAhorro",
                        principalColumn: "IdCajaAhorro",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CajasUsuario_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pago",
                columns: table => new
                {
                    IdPago = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Detalle = table.Column<string>(type: "varchar(50)", nullable: false),
                    Monto = table.Column<float>(type: "real", nullable: false),
                    IsPagado = table.Column<bool>(type: "bit", nullable: false),
                    Metodo = table.Column<string>(type: "varchar(50)", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pago", x => x.IdPago);
                    table.ForeignKey(
                        name: "FK_Pago_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlazoFijo",
                columns: table => new
                {
                    IdPlazoFijo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Monto = table.Column<float>(type: "real", nullable: false),
                    FechaIni = table.Column<DateTime>(type: "date", nullable: false),
                    FechaFin = table.Column<DateTime>(type: "date", nullable: false),
                    Tasa = table.Column<float>(type: "real", nullable: false),
                    IsPagado = table.Column<bool>(type: "bit", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false),
                    CbuAPagar = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlazoFijo", x => x.IdPlazoFijo);
                    table.ForeignKey(
                        name: "FK_PlazoFijo_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tarjeta",
                columns: table => new
                {
                    IdTarjeta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    CodigoV = table.Column<int>(type: "int", nullable: false),
                    Limite = table.Column<float>(type: "real", nullable: false),
                    Consumos = table.Column<float>(type: "real", nullable: false),
                    IdUsuario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarjeta", x => x.IdTarjeta);
                    table.ForeignKey(
                        name: "FK_Tarjeta_Usuario_IdUsuario",
                        column: x => x.IdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "IdUsuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Usuario",
                columns: new[] { "IdUsuario", "Apellido", "Clave", "Dni", "IntentosFallidos", "IsAdmin", "IsBloqueado", "Mail", "Nombre" },
                values: new object[] { 1, "admin", "123", 2, 0, true, false, "admin@admin.com", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_CajasUsuario_IdCajaAhorro",
                table: "CajasUsuario",
                column: "IdCajaAhorro");

            migrationBuilder.CreateIndex(
                name: "IX_Movimiento_IdCajaAhorro",
                table: "Movimiento",
                column: "IdCajaAhorro");

            migrationBuilder.CreateIndex(
                name: "IX_Pago_IdUsuario",
                table: "Pago",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_PlazoFijo_IdUsuario",
                table: "PlazoFijo",
                column: "IdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Tarjeta_IdUsuario",
                table: "Tarjeta",
                column: "IdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CajasUsuario");

            migrationBuilder.DropTable(
                name: "Movimiento");

            migrationBuilder.DropTable(
                name: "Pago");

            migrationBuilder.DropTable(
                name: "PlazoFijo");

            migrationBuilder.DropTable(
                name: "Tarjeta");

            migrationBuilder.DropTable(
                name: "CajaDeAhorro");

            migrationBuilder.DropTable(
                name: "Usuario");
        }
    }
}
