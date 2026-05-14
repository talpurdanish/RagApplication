using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddingDirectorinForiegnKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Actors_DirectorId1",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_DirectorId1",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "DirectorId1",
                table: "Movies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DirectorId1",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_DirectorId1",
                table: "Movies",
                column: "DirectorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Actors_DirectorId1",
                table: "Movies",
                column: "DirectorId1",
                principalTable: "Actors",
                principalColumn: "Id");
        }
    }
}
