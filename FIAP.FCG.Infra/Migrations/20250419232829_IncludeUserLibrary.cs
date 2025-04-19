using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIAP.FCG.Infra.Migrations
{
    /// <inheritdoc />
    public partial class IncludeUserLibrary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserLibrary",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 65, nullable: false),
                    GameId = table.Column<Guid>(type: "uniqueidentifier", maxLength: 80, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDeleted = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLibrary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLibrary_Game_GameId",
                        column: x => x.GameId,
                        principalSchema: "dbo",
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLibrary_UserProfile_UserProfileId",
                        column: x => x.UserProfileId,
                        principalSchema: "dbo",
                        principalTable: "UserProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserLibrary_GameId",
                schema: "dbo",
                table: "UserLibrary",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLibrary_UserProfileId",
                schema: "dbo",
                table: "UserLibrary",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLibrary",
                schema: "dbo");
        }
    }
}
