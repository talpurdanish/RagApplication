using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class removedPokemonAndEmployeetables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Pokemons");

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "Votes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DirectorId1",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieProductionCompanies",
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
                name: "MovieId1",
                table: "MovieKeywords",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GenreId1",
                table: "MovieGenres",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MovieId1",
                table: "MovieGenres",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActorId1",
                table: "MovieCasts",
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
                name: "IX_Movies_DirectorId1",
                table: "Movies",
                column: "DirectorId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCompanies_MovieId1",
                table: "MovieProductionCompanies",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCompanies_ProductionCompanyId1",
                table: "MovieProductionCompanies",
                column: "ProductionCompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieKeywords_KeywordId1",
                table: "MovieKeywords",
                column: "KeywordId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieKeywords_MovieId1",
                table: "MovieKeywords",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_GenreId1",
                table: "MovieGenres",
                column: "GenreId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres",
                column: "MovieId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCasts_ActorId1",
                table: "MovieCasts",
                column: "ActorId1");

            migrationBuilder.CreateIndex(
                name: "IX_MovieCasts_MovieId1",
                table: "MovieCasts",
                column: "MovieId1");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCasts_Actors_ActorId1",
                table: "MovieCasts",
                column: "ActorId1",
                principalTable: "Actors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieCasts_Movies_MovieId1",
                table: "MovieCasts",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Genres_GenreId1",
                table: "MovieGenres",
                column: "GenreId1",
                principalTable: "Genres",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovieKeywords_Keywords_KeywordId1",
                table: "MovieKeywords",
                column: "KeywordId1",
                principalTable: "Keywords",
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
                name: "FK_MovieProductionCompanies_ProductionCompanies_ProductionCompanyId1",
                table: "MovieProductionCompanies",
                column: "ProductionCompanyId1",
                principalTable: "ProductionCompanies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Actors_DirectorId1",
                table: "Movies",
                column: "DirectorId1",
                principalTable: "Actors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Movies_MovieId1",
                table: "Votes",
                column: "MovieId1",
                principalTable: "Movies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovieCasts_Actors_ActorId1",
                table: "MovieCasts");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieCasts_Movies_MovieId1",
                table: "MovieCasts");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Genres_GenreId1",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieGenres_Movies_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieKeywords_Keywords_KeywordId1",
                table: "MovieKeywords");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieKeywords_Movies_MovieId1",
                table: "MovieKeywords");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieProductionCompanies_Movies_MovieId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_MovieProductionCompanies_ProductionCompanies_ProductionCompanyId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Actors_DirectorId1",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Movies_MovieId1",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_MovieId1",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Movies_DirectorId1",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_MovieProductionCompanies_MovieId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropIndex(
                name: "IX_MovieProductionCompanies_ProductionCompanyId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropIndex(
                name: "IX_MovieKeywords_KeywordId1",
                table: "MovieKeywords");

            migrationBuilder.DropIndex(
                name: "IX_MovieKeywords_MovieId1",
                table: "MovieKeywords");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_GenreId1",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieGenres_MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropIndex(
                name: "IX_MovieCasts_ActorId1",
                table: "MovieCasts");

            migrationBuilder.DropIndex(
                name: "IX_MovieCasts_MovieId1",
                table: "MovieCasts");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "DirectorId1",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropColumn(
                name: "ProductionCompanyId1",
                table: "MovieProductionCompanies");

            migrationBuilder.DropColumn(
                name: "KeywordId1",
                table: "MovieKeywords");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieKeywords");

            migrationBuilder.DropColumn(
                name: "GenreId1",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieGenres");

            migrationBuilder.DropColumn(
                name: "ActorId1",
                table: "MovieCasts");

            migrationBuilder.DropColumn(
                name: "MovieId1",
                table: "MovieCasts");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Attrition = table.Column<bool>(type: "bit", nullable: false),
                    BusinessTravel = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DailyRate = table.Column<int>(type: "int", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DescriptionEmbedding = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: true),
                    DistanceFromHome = table.Column<int>(type: "int", nullable: false),
                    Education = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EducationField = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false),
                    EnvironmentSatisfaction = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    HourlyRate = table.Column<int>(type: "int", nullable: false),
                    JobInvolvement = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    JobLevel = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    JobRole = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    JobSatisfaction = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MaritalStatus = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    MonthlyIncome = table.Column<double>(type: "float", nullable: false),
                    MonthlyRate = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NumCompaniesWorked = table.Column<int>(type: "int", nullable: false),
                    Over18 = table.Column<bool>(type: "bit", nullable: false),
                    OverTime = table.Column<bool>(type: "bit", nullable: false),
                    PercentSalaryHike = table.Column<double>(type: "float", nullable: false),
                    PerformanceRating = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RelationshipSatisfaction = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    StandardHours = table.Column<int>(type: "int", nullable: false),
                    StockOptionLevel = table.Column<int>(type: "int", nullable: false),
                    TotalWorkingYears = table.Column<int>(type: "int", nullable: false),
                    TrainingTimesLastYear = table.Column<int>(type: "int", nullable: false),
                    WorkLifeBalance = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    YearsAtCompany = table.Column<int>(type: "int", nullable: false),
                    YearsInCurrentRole = table.Column<int>(type: "int", nullable: false),
                    YearsSinceLastPromotion = table.Column<int>(type: "int", nullable: false),
                    YearsWithCurrManager = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeNumber);
                });

            migrationBuilder.CreateTable(
                name: "Pokemons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Abilities = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Classification = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    DescriptionEmbedding = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemons", x => x.Id);
                });
        }
    }
}
