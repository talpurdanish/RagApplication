using Jina;
using Microsoft.Extensions.AI;

namespace RagWebApi.Service
{
    public interface IJinaClientWrapper
    {
        Task<Embedding<float>> GenerateAsync(string text, EmbeddingGenerationOptions options);
        Task<GeneratedEmbeddings<Embedding<float>>> GenerateMixedEmbeddingsAsync(
            AnyOf<string, TextDoc, ImageDoc>[] items,
            EmbeddingGenerationOptions options);
    }

    public class JinaClientWrapper(JinaClient client) : IJinaClientWrapper
    {
        public Task<Embedding<float>> GenerateAsync(string text, EmbeddingGenerationOptions options)
            => client.GenerateAsync(text, options);

        public Task<GeneratedEmbeddings<Embedding<float>>> GenerateMixedEmbeddingsAsync(
            AnyOf<string, TextDoc, ImageDoc>[] items,
            EmbeddingGenerationOptions options)
            => client.GenerateMixedEmbeddingsAsync(items, options);
    }

}
