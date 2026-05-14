using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RagWebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddedImageMetatdatasTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImageMetadatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalFileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    BlobFileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ThumbnailBlobFileName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Width = table.Column<int>(type: "int", nullable: false),
                    Height = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ThumbnailImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ThumbWidth = table.Column<int>(type: "int", nullable: false),
                    ThumbHeight = table.Column<int>(type: "int", nullable: false),
                    ThumbFormat = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AIInsights = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AnalysisCompleted = table.Column<bool>(type: "bit", nullable: false),
                    ImageEmbedding = table.Column<string>(type: "NVARCHAR(MAX)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageMetadatas", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImageMetadatas");
        }
    }
}
