


using RagWebApi.Viewmodels;

namespace RagWebApi.Service
{
    public class WorkflowNotifier
    {
        public event Func<Task>? StartImageAiAnalysis;
        public event Func<Task>? StartImageEmbeddingsCalculation;
        public event Func<EmbeddingsType, Task>? StartMoviesEmbeddingsCalculation;
        public event Func<Task>? StartDocumentEmbeddingsCalculation;

        public void TriggerImageAiAnalysis() => StartImageAiAnalysis?.Invoke();
        public void TriggerImageEmbeddingsCalculation() => StartImageEmbeddingsCalculation?.Invoke();
        public void TriggerDocumentEmbeddingsCalculation() => StartDocumentEmbeddingsCalculation?.Invoke();

        public event Func<WeightedQuery, Task<List<SearchResults>>>? StartSearchQuery;

        public Task<List<SearchResults>> TriggerSearchQuery(WeightedQuery query)
            => StartSearchQuery != null
                ? StartSearchQuery.Invoke(query)
                : Task.FromResult(new List<SearchResults>());

        public void TriggerMoviesEmbeddingsCalculation(EmbeddingsType type) => StartMoviesEmbeddingsCalculation?.Invoke(type);

    }

    public record SearchResults(int MovieId, double Similarity);

    public enum EmbeddingsType
    {
        jina = 0x01,  jinaCombined  = 0x03
    }

}
