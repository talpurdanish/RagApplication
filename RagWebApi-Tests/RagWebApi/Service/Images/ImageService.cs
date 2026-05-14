using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using RagWebApi.DataContext;
using RagWebApi.Models;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace RagWebApi.Service.Images
{
    public interface IImageService
    {
        Task<IEnumerable<ImageInfoDto>> GetAllImagesAsync();
        Task<ImageInfoDto?> GetImageAsync(int id);
        Task<bool> UploadImageAsync(Stream fileStream, string originalFileName, string contentType);
        void StartAnalysis();
        Task<IEnumerable<ImageInfoDto>> SearchAsync(string query);

        Task<byte[]> GenerateImage(string query, string model);

        Task<string> GetAiInsight(int id);

        Task<bool> UpdateAiInsghts(List<ImageAiInsightResult> results);
        Task<bool> UpdateEmbeddings(List<ImageEmbeddingsResult> results);

    }

    public record ImageAiInsightResult(int ImageId, string AiInsight);
    public record ImageEmbeddingsResult(int ImageId, float[] AiInsight, float[] Embeddings);


    public class ImageService(RagContext context,
        IImageAiService aiService,
        WorkflowNotifier notifier) : IImageService
    {
        public async Task<bool> UploadImageAsync(Stream fileStream, string originalFileName, string contentType)
        {
            var (width, height, format) = GetImageMetadata(fileStream);

            var (thumbStream, thumbWidth, thumbHeight, thumbFormat) = GenerateThumbnail(fileStream, 200, 150);
            var originalExt = Path.GetExtension(originalFileName);
            var uniqueName = $"{Guid.NewGuid()}{originalExt}";
            var thumbName = $"thumb-{uniqueName}";

            fileStream.Position = 0;
            
            var imageBytes = await ConvertPicture(fileStream);

            // Analysis not done here, just set status to Pending
            var metadata = new ImageMetadata
            {
                OriginalFileName = originalFileName,
                BlobFileName = uniqueName,
                ThumbnailBlobFileName = thumbName,
                Image = imageBytes,
                Width = width,
                Height = height,
                Format = format,
                ThumbnailImage = thumbStream.ToArray(),
                ThumbWidth = thumbWidth,
                ThumbHeight = thumbHeight,
                ThumbFormat = thumbFormat,
                FileSizeBytes = fileStream.Length,
                UploadedAt = DateTime.UtcNow,
                AIInsights = null,
                AnalysisCompleted = false
            };

            await context.ImageMetadatas.AddAsync(metadata);
            var rowsdata = await context.SaveChangesAsync();
            if (rowsdata > 0)
            {
                notifier.TriggerImageAiAnalysis();
            }
            return rowsdata > 0;
        }

        public void StartAnalysis()
        {
            notifier.TriggerImageAiAnalysis();
        }


        public async Task<ImageInfoDto?> GetImageAsync(int id)
        {
            var metadata = await context.ImageMetadatas.FindAsync(id);
            if (metadata == null) return null;

            return new ImageInfoDto
            {
                Id = metadata.Id,
                Image = Convert.ToBase64String(metadata.Image),
                ThumbnailImage = Convert.ToBase64String(metadata.ThumbnailImage),
                Width = metadata.Width,
                Height = metadata.Height,
                Format = metadata.Format,
                ThumbWidth = metadata.ThumbWidth,
                ThumbHeight = metadata.ThumbHeight,
                ThumbFormat = metadata.ThumbFormat,
                FileSizeBytes = metadata.FileSizeBytes,
                UploadedAt = metadata.UploadedAt,
                AIInsights = metadata.AnalysisCompleted ? metadata.AIInsights : "Image is being analysed"
            };
        }

        public async Task<IEnumerable<ImageInfoDto>> GetAllImagesAsync()
        {
            try
            {
                var all = await context.ImageMetadatas.ToListAsync();
                return all.Select(m => new ImageInfoDto
                {
                    Id = m.Id,
                    Image = Convert.ToBase64String(m.Image),
                    ThumbnailImage = Convert.ToBase64String(m.ThumbnailImage),
                    Width = m.Width,
                    Height = m.Height,
                    Format = m.Format,
                    ThumbWidth = m.ThumbWidth,
                    ThumbHeight = m.ThumbHeight,
                    ThumbFormat = m.ThumbFormat,
                    FileSizeBytes = m.FileSizeBytes,
                    UploadedAt = m.UploadedAt,
                    AIInsights = m.AIInsights
                });
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<IEnumerable<ImageInfoDto>> SearchAsync(string query)
        {
            try
            {
                var all = await context.ImageMetadatas.OrderBy(e => e.Id).ToListAsync();

                var (success, queryEmbedding) = await aiService.GenerateJinaEmbeddingsAsync(query);

                var results = all
                    .Select(m => new ImageInfoDto
                    {
                        Id = m.Id,
                        Image = Convert.ToBase64String(m.Image),
                        ThumbnailImage = Convert.ToBase64String(m.ThumbnailImage),
                        Width = m.Width,
                        Height = m.Height,
                        Format = m.Format,
                        ThumbWidth = m.ThumbWidth,
                        ThumbHeight = m.ThumbHeight,
                        ThumbFormat = m.ThumbFormat,
                        FileSizeBytes = m.FileSizeBytes,
                        UploadedAt = m.UploadedAt,
                        AIInsights = m.AIInsights,
                        Similarity = m.InsightEmbedding != null && m.ImageEmbedding != null ? ComputeWeightedSimilarity(queryEmbedding, m.InsightEmbedding, m.ImageEmbedding) : 10000
                    })
                    .OrderByDescending(r => r.Similarity)
                    .Take(3)
                    .ToList();

                return results;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<byte[]> GenerateImage(string query, string model)
        {
            return await aiService.GenerateImage(query, model);
        }

        private static double CosineSimilarity(float[] v1, float[]? v2)
        {
            if(v2 == null || v1.Length != v2.Length)
            {
                return 0;
            }
            double dot = 0, mag1 = 0, mag2 = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                dot += v1[i] * v2[i];
                mag1 += v1[i] * v1[i];
                mag2 += v2[i] * v2[i];
            }
            return dot / (Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }

        private static double ComputeWeightedSimilarity(Embedding<float>? query, string? insightEmbeddings, string? imageEmbeddings, double weight = 0.7)
        {

            if (query != null && insightEmbeddings != null && imageEmbeddings != null)
            {
                var q = query.Vector.ToArray();
                var v1 = JsonConvert.DeserializeObject<float[]>(insightEmbeddings);
                var v2 = JsonConvert.DeserializeObject<float[]>(imageEmbeddings);
                var insightSimilarity = CosineSimilarity(q, v1);
                var imageSimilarity = CosineSimilarity(q, v2);

                var weightInsight = 1 - weight;
                var weightImage = weight;

                var weightedSimilarity = weightInsight * insightSimilarity + weightImage * imageSimilarity;
                return weightedSimilarity;
            }

            return 0;


        }
        private static async Task<byte[]> ConvertPicture(Stream fileStream)
        {
            using MemoryStream ms = new();
            await fileStream.CopyToAsync(ms);
            return ms.ToArray();
        }

        private static (int width, int height, string format) GetImageMetadata(Stream imageStream)
        {
            ImageInfo imageInfo = Image.Identify(imageStream);

            var format = imageInfo.Metadata.DecodedImageFormat is null ? "" : imageInfo.Metadata.DecodedImageFormat.Name.ToLower();

            return (imageInfo.Width, imageInfo.Height, format);
        }

        private static (MemoryStream thumbStream, int width, int height, string format) GenerateThumbnail(
     Stream imageStream, int thumbnailWidth, int thumbnailHeight)
        {
            imageStream.Position = 0;
            using Image image = Image.Load(imageStream);

            image.Mutate(ctx =>
            {
                ctx.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Crop, // crop instead of just shrinking
                    Size = new Size(thumbnailWidth, thumbnailHeight),
                    Position = AnchorPositionMode.Center // crop from the center
                });
            });

            var ms = new MemoryStream();
            image.SaveAsPng(ms);
            ms.Position = 0;
            return (ms, thumbnailWidth, thumbnailHeight, "png");
        }

        public async Task<bool> UpdateAiInsghts(List<ImageAiInsightResult> results)
        {
            var ids = results.Select(r => r.ImageId).ToList();

            var images = await context.ImageMetadatas.Where(m => ids.Contains(m.Id)).ToListAsync();

            foreach (var image in images)
            {
                var result = results.FirstOrDefault(r => r.ImageId == image.Id);
                if (result != null)
                {
                    image.AIInsights = result.AiInsight;
                    image.AnalysisCompleted = true;
                    context.Entry<ImageMetadata>(image).State = EntityState.Modified;
                }
            }

            context.ImageMetadatas.UpdateRange(images);
            var rows = await context.SaveChangesAsync();
            var completed = rows == images.Count;
            if (completed) {
                notifier.TriggerImageEmbeddingsCalculation();
            }
            return completed;
        }

        public async Task<bool> UpdateEmbeddings(List<ImageEmbeddingsResult> results)
        {
            var ids = results.Select(r => r.ImageId).ToList();

            var images = await context.ImageMetadatas.Where(m => ids.Contains(m.Id)).ToListAsync();

            foreach (var image in images)
            {
                var result = results.FirstOrDefault(r => r.ImageId == image.Id);
                if (result != null)
                {
                    image.InsightEmbedding = JsonConvert.SerializeObject(result.AiInsight);
                    image.ImageEmbedding = JsonConvert.SerializeObject(result.Embeddings);
                    context.Entry<ImageMetadata>(image).State = EntityState.Modified;
                }
            }

            context.ImageMetadatas.UpdateRange(images);
            var rows = await context.SaveChangesAsync();
            return rows == images.Count;
        }

        public async Task<string> GetAiInsight(int id)
        {
         var (success, insight)  = await aiService.GetAiInsights(id);

            return success && insight is not null ? insight : "Sorry no insight generated for Image no " + id;
        }
    }
}
