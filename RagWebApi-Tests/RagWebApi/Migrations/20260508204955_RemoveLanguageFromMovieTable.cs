using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLanguageFromMovieTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Language",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "LanguageSpoken",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ProductionCountries",
                table: "Movies");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReleaseDate",
                table: "Movies",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ProductionCountries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpokenLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpokenLanguages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MovieProductionCountries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionCountryId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    ProductionCountryId1 = table.Column<int>(type: "int", nullable: true),
                    MovieId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieProductionCountries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieProductionCountries_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieProductionCountries_Movies_MovieId1",
                        column: x => x.MovieId1,
                        principalTable: "Movies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovieProductionCountries_ProductionCountries_ProductionCountryId",
                        column: x => x.ProductionCountryId,
                        principalTable: "ProductionCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieProductionCountries_ProductionCountries_ProductionCountryId1",
                        column: x => x.ProductionCountryId1,
                        principalTable: "ProductionCountries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MovieSpokenLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SpokenLanguageId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    SpokenLanguageId1 = table.Column<int>(type: "int", nullable: true),
                    MovieId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieSpokenLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieSpokenLanguages_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieSpokenLanguages_Movies_MovieId1",
                        column: x => x.MovieId1,
                        principalTable: "Movies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MovieSpokenLanguages_SpokenLanguages_SpokenLanguageId",
                        column: x => x.SpokenLanguageId,
                        principalTable: "SpokenLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieSpokenLanguages_SpokenLanguages_SpokenLanguageId1",
                        column: x => x.SpokenLanguageId1,
                        principalTable: "SpokenLanguages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCountries_MovieId",
                table: "MovieProductionCountries",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCountries_MovieId1",
                table: "MovieProductionCountries",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCountries_ProductionCountryId",
                table: "MovieProductionCountries",
                column: "ProductionCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCountries_ProductionCountryId1",
                table: "MovieProductionCountries",
                column: "ProductionCountryId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSpokenLanguages_MovieId",
                table: "MovieSpokenLanguages",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSpokenLanguages_MovieId1",
                table: "MovieSpokenLanguages",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSpokenLanguages_SpokenLanguageId",
                table: "MovieSpokenLanguages",
                column: "SpokenLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSpokenLanguages_SpokenLanguageId1",
                table: "MovieSpokenLanguages",
                column: "SpokenLanguageId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieProductionCountries");

            migrationBuilder.DropTable(
                name: "MovieSpokenLanguages");

            migrationBuilder.DropTable(
                name: "ProductionCountries");

            migrationBuilder.DropTable(
                name: "SpokenLanguages");

            migrationBuilder.DropColumn(
                name: "ReleaseDate",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Movies",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LanguageSpoken",
                table: "Movies",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductionCountries",
                table: "Movies",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");
        }
    }
}
