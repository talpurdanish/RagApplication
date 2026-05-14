using Microsoft.Extensions.AI;
using RagWebApi.DataContext;

namespace RagWebApi.Service.Images
{
    public class ImageEmbeddingsBackgroundService(IServiceScopeFactory scopeFactory, WorkflowNotifier notifier) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            notifier.StartImageAiAnalysis += RunImageAnalysis;
        }

        private async Task RunImageAnalysis()
        {
            {
                int failureCount = 0;
                using var scope = scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RagContext>();
                var imageAiService = scope.ServiceProvider.GetRequiredService<IImageAiService>();
                var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();


                List<ImageEmbeddingsResult> results = [];
                var images = context.ImageMetadatas
                                 .Where(img => (img.AnalysisCompleted && img.InsightEmbedding == null && img.AIInsights != null) || img.ImageEmbedding == null)
                                 .ToList();
                foreach (var image in images)
                {
                    try
                    {
                        //if (image.ImageEmbedding == null) {

                        //    var (success, vector) = await imageAiService.GenerateImageEmbeddingsAsync(image.Image, image.Id, image.Format);
                        //    if (success)
                        //    {
                        //        image.ImageEmbedding = JsonSerializer.Serialize(vector!.Vector.ToArray());
                        //        context.Entry(image).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        //        context.Update(image);
                        //        await context.SaveChangesAsync(cancellationToken);
                        //    }
                        //    else {
                        //        failureCount++;
                        //    }
                        //}

                        //if (image.InsightEmbedding == null)
                        //{

                        //    var (success, vector) = await imageAiService.GenerateTextEmbeddingsAsync(image.AIInsights!, image.Id);
                        //    if (success)
                        //    {
                        //        image.InsightEmbedding = JsonSerializer.Serialize(vector!.Vector.ToArray());
                        //        context.Entry(image).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        //        context.Update(image);
                        //        await context.SaveChangesAsync(cancellationToken);
                        //    }
                        //    else
                        //    {
                        //        failureCount++;
                        //    }
                        //}


                        if (image.ImageEmbedding == null && image.InsightEmbedding == null)
                        {
                            var (success, embeddings) = await imageAiService.GenerateJinaEmbeddingsAsync(image.Image, image.AIInsights, image.Id);
                            if (success && embeddings != null && embeddings.Count == 2)
                            {
                                results.Add(new ImageEmbeddingsResult(image.Id, embeddings[0].Vector.ToArray(), embeddings[1].Vector.ToArray()));
                            }
                            else
                            {
                                failureCount++;
                            }
                        }

                        if (failureCount >= 3)
                            break;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"Error analyzing image {image.Id}: {ex.Message}");
                    }
                }

                if (results.Count != 0)
                {

                    await imageService.UpdateEmbeddings(results);
                }
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}
