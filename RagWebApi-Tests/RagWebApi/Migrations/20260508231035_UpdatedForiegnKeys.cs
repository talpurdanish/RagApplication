using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedForiegnKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCasts_Movies_MovieId1",
                table: "MovieCasts");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieKeywords_Movies_MovieId1",
                table: "MovieKeywords");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieProductionCompanies_Movies_MovieId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieProductionCountries_Movies_MovieId1",
                table: "MovieProductionCountries");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieSpokenLanguages_Movies_MovieId1",
                table: "MovieSpokenLanguages");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Movies_MovieId1",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_MovieId1",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_MovieSpokenLanguages_MovieId1",
                table: "MovieSpokenLanguages");

            migrationBuilder.DropIndex(
                name: "IX_MovieProductionCountries_MovieId1",
                table: "MovieProductionCountries");

            migrationBuilder.DropIndex(
                name: "IX_MovieProductionCompanies_MovieId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropIndex(
                name: "IX_MovieKeywords_MovieId1",
                table: "MovieKeywords");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieCasts_MovieId1",
                table: "MovieCasts");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieSpokenLanguages");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieProductionCountries");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieKeywords");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieCasts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "Votes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieSpokenLanguages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieProductionCountries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieProductionCompanies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieKeywords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieGenres",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieCasts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_MovieId1",
                table: "Votes",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieSpokenLanguages_MovieId1",
                table: "MovieSpokenLanguages",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCountries_MovieId1",
                table: "MovieProductionCountries",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCompanies_MovieId1",
                table: "MovieProductionCompanies",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieKeywords_MovieId1",
                table: "MovieKeywords",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCasts_MovieId1",
                table: "MovieCasts",
                column: "MovieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCasts_Movies_MovieId1",
                table: "MovieCasts",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieKeywords_Movies_MovieId1",
                table: "MovieKeywords",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieProductionCompanies_Movies_MovieId1",
                table: "MovieProductionCompanies",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieProductionCountries_Movies_MovieId1",
                table: "MovieProductionCountries",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieSpokenLanguages_Movies_MovieId1",
                table: "MovieSpokenLanguages",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Movies_MovieId1",
                table: "Votes",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");
        }
    }
}
