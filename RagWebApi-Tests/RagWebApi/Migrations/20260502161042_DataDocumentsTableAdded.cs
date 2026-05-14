using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class DataDocumentsTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NumberOfPages = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DescriptionEmbedding = table.Column<string>(type: "NVARCHAR(MAX)",  nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataDocuments", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataDocuments");
        }
    }
}
