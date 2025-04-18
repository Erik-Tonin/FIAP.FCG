using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIAP.FCG.Infra.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "UserProfile",
                schema: "userprofile",
                newName: "UserProfile",
                newSchema: "dbo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "userprofile");

            migrationBuilder.RenameTable(
                name: "UserProfile",
                schema: "dbo",
                newName: "UserProfile",
                newSchema: "userprofile");
        }
    }
}
