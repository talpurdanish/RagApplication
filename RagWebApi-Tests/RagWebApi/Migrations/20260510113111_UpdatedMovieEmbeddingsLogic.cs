using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedMovieEmbeddingsLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CombinedEmbeddingsId",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MovieEmbeddings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MovieEmbeddingsCombined",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MovieId = table.Column<int>(type: "int", nullable: false),
                    FullTextEmbeddings = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieEmbeddingsCombined", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieEmbeddingsCombined_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieEmbeddingsCombined_MovieId",
                table: "MovieEmbeddingsCombined",
                column: "MovieId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieEmbeddingsCombined");

            migrationBuilder.DropColumn(
                name: "CombinedEmbeddingsId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "MovieEmbeddings");
        }
    }
}
