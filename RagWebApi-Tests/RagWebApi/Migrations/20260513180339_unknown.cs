using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class unknown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "MovieEmbeddingsCombined");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "MovieEmbeddings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MovieEmbeddingsCombined",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "MovieEmbeddings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
