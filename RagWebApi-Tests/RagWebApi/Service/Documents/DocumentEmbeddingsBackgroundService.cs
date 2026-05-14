using Microsoft.Extensions.AI;
using RagWebApi.DataContext;

namespace RagWebApi.Service.Documents
{
    public class DocumentEmbeddingsBackgroundService(IServiceScopeFactory scopeFactory, WorkflowNotifier notifier) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            notifier.StartDocumentEmbeddingsCalculation += RunDocumentAnalysis;
        }

        private async Task RunDocumentAnalysis()
        {
            {
                int failureCount = 0;
                using var scope = scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RagContext>();
                var documentAiService = scope.ServiceProvider.GetRequiredService<IDocumentAiService>();
                var documentService = scope.ServiceProvider.GetRequiredService<IDocumentService>();


                List<DocumentEmbeddingsResult> results = [];
                var documents = context.DataDocuments
                                 .Where(d => d.DescriptionEmbedding == null)
                                 .ToList();

                foreach (var batch in documents.Chunk(5))
                {
                    try
                    {
                        var documentsArray = batch.Select(b => b.ToString()).ToList();
                        var documentIdArray = batch.Select(b => b.Id).ToList();

                        if (documentsArray == null)
                            break;

                        var (success, embeddings) = await documentAiService.GenerateJinaEmbeddingsAsync(documentsArray!, documentIdArray);
                        if (success && embeddings != null && embeddings.Count == 2)
                        {
                            var deResults = embeddings.Zip(documentIdArray, (emb, id) => new DocumentEmbeddingsResult(id, emb.Vector.ToArray()));
                            results.AddRange(deResults);
                        }
                        else
                        {
                            failureCount++;
                        }
                        if (failureCount >= 3)
                            break;
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException($"Error analyzing documents : {ex.Message}");
                    }
                }

                if (results.Count != 0)
                {

                    await documentService.UpdateEmbeddings(results);
                }
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }
    }
}
