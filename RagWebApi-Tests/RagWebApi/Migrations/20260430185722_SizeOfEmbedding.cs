using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class SizeOfEmbedding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEmbedding",
                table: "Pokemons",
                type: "NVARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEmbedding",
                table: "Pokemons",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)",
                oldNullable: true);
        }
    }
}
