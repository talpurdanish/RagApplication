using Jina;
using Microsoft.Extensions.AI;


namespace RagWebApi.Service.Movies
{

    public interface IMovieAiService
    {
        Task<(bool, Embedding<float>?)> GenerateJinaEmbeddingsAsync(string text);
        Task<(bool, GeneratedEmbeddings<Embedding<float>>?)> GenerateJinaEmbeddingsAsync(string[] texts, int movieId);
        Task<(bool, GeneratedEmbeddings<Embedding<float>>?)> GenerateJinaEmbeddingsAsync(string text, int movieId);

    }
    public class MovieAiService(IJinaClientWrapper jinaWrapper) : IMovieAiService
    {

        

        public async Task<(bool, Embedding<float>?)> GenerateJinaEmbeddingsAsync(string text)
        {
            int retry = 0;
            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Processing {text} retry attempt {retry + 1}");
                Console.ForegroundColor = ConsoleColor.White;

                try
                {

                    var embeddings = await jinaWrapper.GenerateAsync(
                                            text,
                                            new EmbeddingGenerationOptions
                                            {
                                                Dimensions = 256,
                                            });

                    
                    return (true, embeddings);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                retry++;

                await Task.Delay(TimeSpan.FromMilliseconds(500));
            } while (retry < 3);

            return (false, null);
        }
        public async Task<(bool, GeneratedEmbeddings<Embedding<float>>?)> GenerateJinaEmbeddingsAsync(string text, int movieId) {

            int retry = 0;
            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Processing {movieId} retry attempt {retry + 1}");
                Console.ForegroundColor = ConsoleColor.White;

                try
                {

                    var embeddings = await jinaWrapper.GenerateAsync(
                                            text,
                                            new EmbeddingGenerationOptions
                                            {
                                                Dimensions = 256,
                                            });

                    var generatedEmbeddings = new GeneratedEmbeddings<Embedding<float>>(new List<Embedding<float>>() { embeddings });
                    return (true, generatedEmbeddings);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                retry++;

                await Task.Delay(TimeSpan.FromMilliseconds(500));
            } while (retry < 3);

            return (false, null);

        }
        public async Task<(bool, GeneratedEmbeddings<Embedding<float>>?)> GenerateJinaEmbeddingsAsync(string[] texts, int movieId)
        {
            int retry = 0;
            do
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Processing Movie with Id {movieId} retry attempt {retry + 1}");
                Console.ForegroundColor = ConsoleColor.White;

                try
                {
                    List<AnyOf<string, TextDoc, ImageDoc>> items = texts.Select(t => new AnyOf<string, TextDoc, ImageDoc>(t)).ToList();

                    var embeddings = await jinaWrapper.GenerateMixedEmbeddingsAsync(
                                            items.ToArray(),
                                            new EmbeddingGenerationOptions
                                            {
                                                Dimensions = 256,
                                            });

                    return (true, embeddings);

                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {e.Message}");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                retry++;

                await Task.Delay(TimeSpan.FromMilliseconds(500));
            } while (retry < 3);

            return (false, null);
        }
    }
}
