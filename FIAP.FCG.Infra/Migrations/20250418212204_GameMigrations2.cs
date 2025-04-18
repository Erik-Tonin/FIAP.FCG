using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FIAP.FCG.Infra.Migrations
{
    /// <inheritdoc />
    public partial class GameMigrations2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "Category",
                columns: new[] { "Id", "DateCreated", "DateDeleted", "DateUpdated", "Name" },
                values: new object[,]
                {
                    { new Guid("20581836-2641-4794-8d3c-4f13bb6c9d53"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Ação" },
                    { new Guid("33e644e9-3be5-4490-98be-82ed04ed28a9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Comedia" },
                    { new Guid("a3ff5d49-2a58-44d4-aad6-e1c151db7aea"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Terror" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("20581836-2641-4794-8d3c-4f13bb6c9d53"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("33e644e9-3be5-4490-98be-82ed04ed28a9"));

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "Category",
                keyColumn: "Id",
                keyValue: new Guid("a3ff5d49-2a58-44d4-aad6-e1c151db7aea"));
        }
    }
}
