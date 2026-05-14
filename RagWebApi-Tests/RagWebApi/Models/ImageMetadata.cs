using System.ComponentModel.DataAnnotations;

namespace RagWebApi.Models
{
    public class ImageMetadata
    {
        [Key]
        public int Id { get; set; }
        public string OriginalFileName { get; set; } = string.Empty;
        public string BlobFileName { get; set; } = string.Empty;
        public string ThumbnailBlobFileName { get; set; } = string.Empty;
        public byte[] Image { get; set; } = [];
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; } = string.Empty;
        public byte[] ThumbnailImage { get; set; } = [];
        public int ThumbWidth { get; set; }
        public int ThumbHeight { get; set; }
        public string ThumbFormat { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        [MaxLength]
        public string? AIInsights { get; set; }
        public bool AnalysisCompleted { get; set; }
        [MaxLength]
        public string? InsightEmbedding { get; set; }
        [MaxLength]
        public string? ImageEmbedding { get; set; }

    }

    public class ImageInfoDto
    {
        public int Id { get; set; }
        public string Image { get; set; } = string.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Format { get; set; } = string.Empty;
        public string ThumbnailImage { get; set; } = string.Empty;
        public int ThumbWidth { get; set; }
        public int ThumbHeight { get; set; }
        public string ThumbFormat { get; set; } = string.Empty;
        public long FileSizeBytes { get; set; }
        public DateTime UploadedAt { get; set; }
        public string? AIInsights { get; set; }

        public double Similarity { get; set; }
    }
}
