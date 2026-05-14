using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Newtonsoft.Json;
using RagWebApi.DataContext;
using RagWebApi.Models;
using RagWebApi.Viewmodels;

namespace RagWebApi.Service.Movies
{
    public class MovieSearchBackgroundService(IServiceScopeFactory scopeFactory, WorkflowNotifier notifier) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            notifier.StartSearchQuery += ComputeSearchResultsAsync;
        }
        private static double CosineSimilarity(float[] v1, float[]? v2)
        {
            if (v2 == null || v1.Length != v2.Length)
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
        public static double JaccardSimilarity<T>(IEnumerable<T> setA, IEnumerable<T> setB)
        {
            if (setA == null || setB == null) return 0;

            var hashA = new HashSet<T>(setA);
            var hashB = new HashSet<T>(setB);

            if (hashA.Count == 0 && hashB.Count == 0) return 0;

            var intersection = new HashSet<T>(hashA);
            intersection.IntersectWith(hashB);

            var union = new HashSet<T>(hashA);
            union.UnionWith(hashB);

            return union.Count > 0 ? (double)intersection.Count / union.Count : 0;
        }

        public static double Normalize(double value, double min, double max)
        {
            if (max <= min) return 0; // avoid divide by zero
            if (value < min) value = min;
            if (value > max) value = max;

            return (value - min) / (max - min);
        }

        private static double ComputeWeightedSimilarity(Embedding<float> query, float[] embeddings, Movie? mv)
        {
            var similarity = CosineSimilarity(query.Vector.ToArray(), embeddings);

            var nPopularity = mv is not null ? Normalize(mv.Popularity, 0, 200) : 0;
            var nVoteAverage = mv is not null ? Normalize(mv.VoteAverage, 0, 10) : 0;

            return 0.7 * similarity + 0.15 * nPopularity + 0.15 * nVoteAverage;
        }

        private static double ComputeWeightedSimilarity(Embedding<float> query, List<WeightedEmbeddings> embeddings, Movie? mv)
        {
            var weightedScores = embeddings
                .Where(e => e.Weight > 0)
                .Select(e => CosineSimilarity(query.Vector.ToArray(), e.Embeddings) * e.Weight);

            var nPopularity = mv is not null ? Normalize(mv.Popularity, 0, 200) : 0;
            var nVoteAverage = mv is not null ? Normalize(mv.VoteAverage, 0, 10) : 0;
            double sumWeights = embeddings.Where(e => e.Weight > 0).Sum(e => e.Weight);
            var normalizedSimilarity = sumWeights > 0 ? weightedScores.Sum() / sumWeights : 0;
            return 0.7 * normalizedSimilarity + 0.15 * nPopularity + 0.15 * nVoteAverage;
        }

        private async Task<List<SearchResults>> ComputeSearchResultsAsync(WeightedQuery query)
        {
            {
                EmbeddingsType eType = (EmbeddingsType)query.Type;

                var isCombined = eType == EmbeddingsType.jinaCombined;

                using var scope = scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<RagContext>();
                var moviesAiService = scope.ServiceProvider.GetRequiredService<IMovieAiService>();
                var moviesService = scope.ServiceProvider.GetRequiredService<IMovieService>();

                var (success, queryEmbeddings) = await moviesAiService.GenerateJinaEmbeddingsAsync(query.Query);

                if (!success || queryEmbeddings == null)
                    return [];
                List<SearchResults> results = isCombined ?
                    await ProcessSearch(context, queryEmbeddings) :
                    await ProcessSearch(context, query, queryEmbeddings);

                return await Task.FromResult(results);

            }
        }

        private static async Task<List<SearchResults>> ProcessSearch(RagContext context, WeightedQuery query, Embedding<float> queryEmbeddings)
        {

            List<SearchResults> results = [];
            var movieEmbeddings = await context.MovieEmbeddings.ToListAsync();
            var movies = await context.Movies.ToListAsync();
            foreach (var movie in movieEmbeddings)
            {
                try
                {
                    var mv = movies.Where(m => m.Id == movie.Id).FirstOrDefault();
                    //List<WeightedEmbeddings> weightedEmbeddings = [
                    //    new WeightedEmbeddings(0.01, movie.Budget),
                    //    new WeightedEmbeddings(0.01, movie.Homepage),
                    //    new WeightedEmbeddings(0.2, movie.Title),
                    //    new WeightedEmbeddings(0.2, movie.Overview),
                    //    new WeightedEmbeddings(0.0625, movie.Popularity),
                    //    new WeightedEmbeddings(0.035, movie.Status),
                    //    new WeightedEmbeddings(0.08, movie.Tagline),
                    //    new WeightedEmbeddings(0.02, movie.VoteAverage),
                    //    new WeightedEmbeddings(0.02, movie.VoteCount),
                    //    new WeightedEmbeddings(0.03, movie.DirectorName),
                    //    new WeightedEmbeddings(0.07, movie.Genres),
                    //    new WeightedEmbeddings(0.07, movie.Casts),
                    //    new WeightedEmbeddings(0.07, movie.Keywords),
                    //    new WeightedEmbeddings(0.03, movie.ProductionCompanies),
                    //    new WeightedEmbeddings(0.0625, movie.ProductionCountries),
                    //    new WeightedEmbeddings(0.03, movie.SpokenLanguages),
                    //];

                    List<WeightedEmbeddings> weightedEmbeddings = [
                        new WeightedEmbeddings(query.Budget, movie.Budget),
                            new WeightedEmbeddings(query.Homepage, movie.Homepage),
                            new WeightedEmbeddings(query.Title, movie.Title),
                            new WeightedEmbeddings(query.Overview, movie.Overview),
                            new WeightedEmbeddings(query.Popularity, movie.Popularity),
                            new WeightedEmbeddings(query.Status, movie.Status),
                            new WeightedEmbeddings(query.Tagline, movie.Tagline),
                            new WeightedEmbeddings(query.VoteAverage, movie.VoteAverage),
                            new WeightedEmbeddings(query.VoteCount, movie.VoteCount),
                            new WeightedEmbeddings(query.DirectorName, movie.DirectorName),
                            new WeightedEmbeddings(query.Genres, movie.Genres),
                            new WeightedEmbeddings(query.Casts, movie.Casts),
                            new WeightedEmbeddings(query.Keywords, movie.Keywords),
                            new WeightedEmbeddings(query.ProductionCompanies, movie.ProductionCompanies),
                            new WeightedEmbeddings(query.ProductionCountries, movie.ProductionCountries),
                            new WeightedEmbeddings(query.SpokenLanguages, movie.SpokenLanguages),
                        ];

                    var searchResult = ComputeWeightedSimilarity(queryEmbeddings, weightedEmbeddings, mv);

                    results.Add(new SearchResults(movie.MovieId, searchResult));

                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error analyzing movie {movie.Id}: {ex.Message}");
                }


            }
            return results;
        }

        private static async Task<List<SearchResults>> ProcessSearch(RagContext context, Embedding<float> queryEmbeddings)
        {

            List<SearchResults> results = [];
            var movieEmbeddings = await context.MovieEmbeddingsCombined.ToListAsync();
            var movies = await context.Movies.ToListAsync();
            foreach (var movie in movieEmbeddings)
            {
                try
                {
                    var mv = movies.Where(m => m.Id == movie.Id).FirstOrDefault();
                    var embeddings = JsonConvert.DeserializeObject<float[]>(movie.FullTextEmbeddings);
                    if (embeddings is null)
                        continue;
                    var searchResult = ComputeWeightedSimilarity(queryEmbeddings, embeddings, mv);
                    results.Add(new SearchResults(movie.MovieId, searchResult));

                }
                catch (Exception ex)
                {
                    throw new ArgumentException($"Error analyzing movie {movie.Id}: {ex.Message}");
                }


            }
            return results;
        }
    }
    public class WeightedEmbeddings(double weight, string embeddingsJson)
    {
        public double Weight { get; private set; } = weight;
        public float[] Embeddings { get; private set; } = ConvertEmbeddings(embeddingsJson);
        private static float[] ConvertEmbeddings(string json)
        {
            var floats = JsonConvert.DeserializeObject<float[]>(json);
            return json != null && floats != null ? floats : [];

        }

    }

}
