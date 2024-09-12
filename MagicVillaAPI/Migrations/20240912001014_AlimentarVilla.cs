using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaAPI.Migrations
{
    public partial class AlimentarVilla : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenURL", "Nombre", "Tarifa" },
                values: new object[] { 1, "Ubicado en Villafranca", new DateTime(2024, 9, 11, 18, 10, 14, 93, DateTimeKind.Local).AddTicks(7087), new DateTime(2024, 9, 11, 18, 10, 14, 93, DateTimeKind.Local).AddTicks(7110), "", "Villa Real", 200.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
