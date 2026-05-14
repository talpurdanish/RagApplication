using RagWebApi.DataContext;

namespace RagWebApi.Service.Images
{
    public class ImageAnalysisBackgroundService(IServiceScopeFactory scopeFactory, WorkflowNotifier notifier) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            notifier.StartImageAiAnalysis += RunImageAnalysis;
        }

        private async Task RunImageAnalysis()
        {

            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RagContext>();
            var imageAiService = scope.ServiceProvider.GetRequiredService<IImageAiService>();
            var imageService = scope.ServiceProvider.GetRequiredService<IImageService>();


            List<ImageAiInsightResult> results = [];
            var images = context.ImageMetadatas
                             .Where(img => !img.AnalysisCompleted)
                             .ToList();
            foreach (var image in images)
            {
                try
                {
                    var (success, insight) = await imageAiService.GetAiInsights(image.Id);
                    if (success)
                    {
                        results.Add(new ImageAiInsightResult(image.Id, insight));
                    }
                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error analyzing image {image.Id}: {ex.Message}");
                }
            }

            if (results.Count != 0)
            {
                await imageService.UpdateAiInsghts(results);
                notifier.TriggerImageEmbeddingsCalculation();
            }

            await Task.Delay(TimeSpan.FromSeconds(10));
        }
    }


}
