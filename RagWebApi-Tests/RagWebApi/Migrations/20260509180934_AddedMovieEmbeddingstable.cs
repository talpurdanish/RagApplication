using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedMovieEmbeddingstable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Embeddings",
                table: "Movies");

            migrationBuilder.AddColumn<int>(
                name: "EmbeddingsId",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MovieEmbeddings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    Budget = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Homepage = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Overview = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Popularity = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Tagline = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    VoteAverage = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    VoteCount = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    DirectorName = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Genres = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Casts = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Keywords = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    ProductionCompanies = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    ProductionCountries = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    SpokenLanguages = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieEmbeddings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieEmbeddings_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieEmbeddings_MovieId",
                table: "MovieEmbeddings",
                column: "MovieId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieEmbeddings");

            migrationBuilder.DropColumn(
                name: "EmbeddingsId",
                table: "Movies");

            migrationBuilder.AddColumn<string>(
                name: "Embeddings",
                table: "Movies",
                type: "NVARCHAR(MAX)",
                maxLength: 256,
                nullable: true);
        }
    }
}
