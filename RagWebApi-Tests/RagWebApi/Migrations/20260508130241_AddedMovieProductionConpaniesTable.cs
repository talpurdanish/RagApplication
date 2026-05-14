using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedMovieProductionConpaniesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_ProductionCompanies_ProductionCompanyId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ProductionCompanyId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ProductionCompanyId",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "MovieProductionCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductionCompanyId = table.Column<int>(type: "int", nullable: false),
                    MovieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieProductionCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieProductionCompanies_Movies_MovieId",
                        column: x => x.MovieId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieProductionCompanies_ProductionCompanies_ProductionCompanyId",
                        column: x => x.ProductionCompanyId,
                        principalTable: "ProductionCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCompanies_MovieId",
                table: "MovieProductionCompanies",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieProductionCompanies_ProductionCompanyId",
                table: "MovieProductionCompanies",
                column: "ProductionCompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieProductionCompanies");

            migrationBuilder.AddColumn<int>(
                name: "ProductionCompanyId",
                table: "Movies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ProductionCompanyId",
                table: "Movies",
                column: "ProductionCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_ProductionCompanies_ProductionCompanyId",
                table: "Movies",
                column: "ProductionCompanyId",
                principalTable: "ProductionCompanies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
