using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using RagWebApi.DataContext;
using System.Collections.Concurrent;

namespace RagWebApi.Service.Movies
{
    public class MovieEmbeddingsBackgroundService(IServiceScopeFactory scopeFactory, WorkflowNotifier notifier) : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            notifier.StartMoviesEmbeddingsCalculation += RunMovieAnalysis;
        }

        private async Task RunMovieAnalysis(EmbeddingsType type)
        {

            var isJina = type == EmbeddingsType.jina || type == EmbeddingsType.jinaCombined;

            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RagContext>();
            var moviesAiService = scope.ServiceProvider.GetRequiredService<IMovieAiService>();
            var moviesService = scope.ServiceProvider.GetRequiredService<IMovieService>();

            var results = new ConcurrentBag<MovieEmbeddingsResult>();
            int failureCount = 0;
            int failureThreshold = 3;
            var cts = new CancellationTokenSource();
            var semaphore = new SemaphoreSlim(1);
            var movies = await context.Movies.ToListAsync(cts.Token);

            var tasks = movies.Select(async movie =>
            {
                if (cts.Token.IsCancellationRequested)
                    return;
                await semaphore.WaitAsync(cts.Token);
                try
                {
                    var (success, embeddings) = type switch
                    {
                        EmbeddingsType.jina => await moviesAiService.GenerateJinaEmbeddingsAsync(movie.ToStringArray(), movie.Id),
                        EmbeddingsType.jinaCombined => await moviesAiService.GenerateJinaEmbeddingsAsync(movie.ToString(), movie.Id),
                        _ => (false, null)
                    };

                    if (success && embeddings != null)
                    {
                        results.Add(new MovieEmbeddingsResult(movie.Id, embeddings.Select(e => e.Vector.ToArray()).ToArray()));
                    }
                    else
                    {
                        if (Interlocked.Increment(ref failureCount) >= failureThreshold)
                            cts.Cancel();
                        Console.WriteLine($"No results returned for {failureCount} times");
                    }

                }
                catch (Exception ex)
                {
                    if (Interlocked.Increment(ref failureCount) >= failureThreshold)
                        cts.Cancel();

                    Console.WriteLine($"Error analyzing movie {movie.Id}: {ex.Message}");
                }
                finally
                {
                    semaphore.Release();
                }

            });

            try
            {
                await Task.WhenAll(tasks);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation cancelled due to repeated failures");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }

            if (!results.IsEmpty)
            {
                await moviesService.UpdateEmbeddings(results.ToList(), type);
            }

        }
    }
}
