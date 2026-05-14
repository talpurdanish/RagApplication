using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddingForiegnKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCasts_Actors_ActorId1",
                table: "MovieCasts");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenreId1",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieKeywords_Keywords_KeywordId1",
                table: "MovieKeywords");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieProductionCompanies_ProductionCompanies_ProductionCompanyId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieProductionCountries_ProductionCountries_ProductionCountryId1",
                table: "MovieProductionCountries");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieSpokenLanguages_SpokenLanguages_SpokenLanguageId1",
                table: "MovieSpokenLanguages");

            migrationBuilder.DropIndex(
                name: "IX_MovieSpokenLanguages_SpokenLanguageId1",
                table: "MovieSpokenLanguages");

            migrationBuilder.DropIndex(
                name: "IX_MovieProductionCountries_ProductionCountryId1",
                table: "MovieProductionCountries");

            migrationBuilder.DropIndex(
                name: "IX_MovieProductionCompanies_ProductionCompanyId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropIndex(
                name: "IX_MovieKeywords_KeywordId1",
                table: "MovieKeywords");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_GenreId1",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieCasts_ActorId1",
                table: "MovieCasts");

            migrationBuilder.DropColumn(
                name: "SpokenLanguageId1",
                table: "MovieSpokenLanguages");

            migrationBuilder.DropColumn(
                name: "ProductionCountryId1",
                table: "MovieProductionCountries");

            migrationBuilder.DropColumn(
                name: "ProductionCompanyId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropColumn(
                name: "KeywordId1",
                table: "MovieKeywords");

            migrationBuilder.DropColumn(
                name: "GenreId1",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "ActorId1",
                table: "MovieCasts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpokenLanguageId1",
                table: "MovieSpokenLanguages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductionCountryId1",
                table: "MovieProductionCountries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductionCompanyId1",
                table: "MovieProductionCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KeywordId1",
                table: "MovieKeywords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenreId1",
                table: "MovieGenres",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActorId1",
                table: "MovieCasts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovieSpokenLanguages_SpokenLanguageId1",
                table: "MovieSpokenLanguages",
                column: "SpokenLanguageId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCountries_ProductionCountryId1",
                table: "MovieProductionCountries",
                column: "ProductionCountryId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCompanies_ProductionCompanyId1",
                table: "MovieProductionCompanies",
                column: "ProductionCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieKeywords_KeywordId1",
                table: "MovieKeywords",
                column: "KeywordId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreId1",
                table: "MovieGenres",
                column: "GenreId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCasts_ActorId1",
                table: "MovieCasts",
                column: "ActorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCasts_Actors_ActorId1",
                table: "MovieCasts",
                column: "ActorId1",
                principalTable: "Actors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenreId1",
                table: "MovieGenres",
                column: "GenreId1",
                principalTable: "Genres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieKeywords_Keywords_KeywordId1",
                table: "MovieKeywords",
                column: "KeywordId1",
                principalTable: "Keywords",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieProductionCompanies_ProductionCompanies_ProductionCompanyId1",
                table: "MovieProductionCompanies",
                column: "ProductionCompanyId1",
                principalTable: "ProductionCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieProductionCountries_ProductionCountries_ProductionCountryId1",
                table: "MovieProductionCountries",
                column: "ProductionCountryId1",
                principalTable: "ProductionCountries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSpokenLanguages_SpokenLanguages_SpokenLanguageId1",
                table: "MovieSpokenLanguages",
                column: "SpokenLanguageId1",
                principalTable: "SpokenLanguages",
                principalColumn: "Id");
        }
    }
}
