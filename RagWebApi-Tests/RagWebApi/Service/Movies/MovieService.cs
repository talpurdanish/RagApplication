using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RagWebApi.DataContext;
using RagWebApi.Models;
using RagWebApi.Viewmodels;
using System.Globalization;

namespace RagWebApi.Service.Movies
{
    public interface IMovieService
    {
        Task<PagedResults> GetAllMoviesAsync(DataFilter filter);
        Task<MovieViewModel?> GetMovieAsync(int id);
        Task<bool> UploadMoviesAsync(Stream fileStream);
        Task StartAnalysis(int type);
        Task<PagedResults> SearchAsync(WeightedQuery query);

        Task<IEnumerable<MovieViewModel>> RecommendAsync(int id);
        Task<bool> UpdateEmbeddings(List<MovieEmbeddingsResult> results, EmbeddingsType type);

        Task<bool> DeleteAllEmbeddings(EmbeddingsType type);
    }

    public record MovieEmbeddingResult(int MovieId, float[] MovieEmbeddings);
    public record MovieEmbeddingsResult(int MovieId, float[][] MovieEmbeddings);
    public class MovieService(RagContext context, WorkflowNotifier notifier) : IMovieService
    {
        private IQueryable<MovieViewModel> GetBaseQuery()
        {

            IQueryable<MovieViewModel> query = context.Movies
                    .Include(m => m.Director)
                    .Include(m => m.MovieCasts)
                        .ThenInclude(mc => mc.Actor)
                    .Include(m => m.MovieGenres)
                        .ThenInclude(mg => mg.Genre)
                    .Include(m => m.MovieKeywords)
                        .ThenInclude(mk => mk.Keyword)
                    .Include(m => m.MovieProductionCompanies)
                        .ThenInclude(mpc => mpc.ProductionCompany)
                    .Include(m => m.Votes)
                    .Select(m => new MovieViewModel
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Overview = m.Overview,
                        Budget = m.Budget,
                        Homepage = m.Homepage,
                        Popularity = m.Popularity,
                        ReleaseDate = m.ReleaseDate,
                        Revenue = m.Revenue,
                        Runtime = m.Runtime,
                        Status = m.Status,
                        Tagline = m.Tagline,
                        VoteAverage = m.VoteAverage,
                        VoteCount = m.VoteCount,
                        DirectorId = m.DirectorId,
                        Director = m.Director != null ? m.Director.Name : "",
                        Countries = m.MovieProductionCountries.Select(mpc => new ProductionCountry
                        {
                            Id = mpc.ProductionCountry != null ? mpc.ProductionCountry.Id : 0,
                            Name = mpc.ProductionCountry != null ? mpc.ProductionCountry.Name : ""
                        }).ToList(),
                        SpokenLanguages = m.MovieSpokenLanguages.Select(msl => new SpokenLanguage
                        {
                            Id = msl.SpokenLanguage != null ? msl.SpokenLanguage.Id : 0,
                            Name = msl.SpokenLanguage != null ? msl.SpokenLanguage.Name : ""
                        }).ToList(),
                        Companies = m.MovieProductionCompanies.Select(mpc => new ProductionCompany
                        {
                            Id = mpc.ProductionCompany != null ? mpc.ProductionCompany.Id : 0,
                            Name = mpc.ProductionCompany != null ? mpc.ProductionCompany.Name : ""
                        }).ToList(),
                        Genres = m.MovieGenres.Select(mg => new Genre
                        {
                            Id = mg.Genre != null ? mg.Genre.Id : 0,
                            Name = mg.Genre != null ? mg.Genre.Name : ""
                        }).ToList(),
                        Keywords = m.MovieKeywords.Select(mk => new Keyword
                        {
                            Id = mk.Keyword != null ? mk.Keyword.Id : 0,
                            Name = mk.Keyword != null ? mk.Keyword.Name : ""
                        }).ToList(),
                        Actors = m.MovieCasts.Select(mc => new Actor
                        {
                            Id = mc.Actor != null ? mc.Actor.Id : 0,
                            Name = mc.Actor != null ? mc.Actor.Name : ""
                        }).ToList()
                    }).AsNoTracking().AsSplitQuery();

            return query;
        }


        public async Task<PagedResults> GetAllMoviesAsync(DataFilter filter)
        {
            var query = context.Movies.
                Include(m => m.MovieCasts)
                .ThenInclude(mc => mc.Actor)
                 .Include(m => m.MovieKeywords)
                        .ThenInclude(mk => mk.Keyword)
                .Select(m => new MiniMovieViewModel()
                {
                    Title = m.Title,
                    Keywords = m.MovieKeywords.Select(mk => new Keyword
                    {
                        Id = mk.Keyword != null ? mk.Keyword.Id : 0,
                        Name = mk.Keyword != null ? mk.Keyword.Name : ""
                    }).ToList(),
                    Actors = m.MovieCasts.Select(mc => new Actor
                    {
                        Id = mc.Actor != null ? mc.Actor.Id : 0,
                        Name = mc.Actor != null ? mc.Actor.Name : ""
                    }).ToList()

                }).AsNoTracking().AsSplitQuery();
            var totalCount = await query.CountAsync();
            var movies = await query.Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize).ToListAsync();

            var pagedResults = new PagedResults(movies, filter.Page, totalCount, filter.PageSize);

            return pagedResults;
        }

        public async Task<MovieViewModel?> GetMovieAsync(int id)
        {
            var query = GetBaseQuery();
            return await query.Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MovieViewModel>> RecommendAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResults> SearchAsync(WeightedQuery query)
        {
            var movieQuery = context.Movies.
                Include(m => m.MovieCasts)
                .ThenInclude(mc => mc.Actor)
                 .Include(m => m.MovieKeywords)
                        .ThenInclude(mk => mk.Keyword)
                .Select(m => new MiniMovieViewModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Keywords = m.MovieKeywords.Select(mk => new Keyword
                    {
                        Id = mk.Keyword != null ? mk.Keyword.Id : 0,
                        Name = mk.Keyword != null ? mk.Keyword.Name : ""
                    }).ToList(),
                    Actors = m.MovieCasts.Select(mc => new Actor
                    {
                        Id = mc.Actor != null ? mc.Actor.Id : 0,
                        Name = mc.Actor != null ? mc.Actor.Name : ""
                    }).ToList()

                }).AsNoTracking().AsSplitQuery();
            List<SearchResults>? searchresults = await notifier.TriggerSearchQuery(query);
            var ids = searchresults.Select(s => s.MovieId).ToList();
            var movies = await movieQuery.Where(m => ids.Contains(m.Id)).ToListAsync();

            var enrichedMovies = movies
                 .Select(m => new MiniMovieViewModel
                 {
                     Id = m.Id,
                     Title = m.Title,
                     Keywords = m.Keywords,
                     Actors = m.Actors,
                     Similarity = searchresults.FirstOrDefault(s => s.MovieId == m.Id)?.Similarity ?? 0f
                 })
                 .OrderByDescending(m => m.Similarity)
                 .Take(5)
                 .ToList();


            return new PagedResults(enrichedMovies);
        }




        public async Task StartAnalysis(int type)
        {
            var eType = (EmbeddingsType)type;
            if (await DeleteAllEmbeddings(eType))
                notifier.TriggerMoviesEmbeddingsCalculation(eType);
        }

        public async Task<bool> UploadMoviesAsync(Stream fileStream)
        {
            try
            {
                using CsvReader csv = new(new StreamReader(fileStream), CultureInfo.InvariantCulture);
                var records = await csv.GetRecordsAsync<dynamic>().ToListAsync();

                var directors = records.Select(r => new CsvItem(int.Parse(r.index), r.director)).ToList();
                //Space Seperated values
                var genres = records.Select(r => new CsvItem(int.Parse(r.index), r.genres)).ToList();
                var actors = records.Select(r => new CsvItem(int.Parse(r.index), r.cast)).ToList();
                var keywords = records.Select(r => new CsvItem(int.Parse(r.index), r.keywords)).ToList();

                //Json values
                var productionCompanies = records.Select(r => new CsvItem(int.Parse(r.index), r.production_companies)).ToList();
                var productionCountries = records.Select(r => new CsvItem(int.Parse(r.index), r.production_countries)).ToList();
                var spokenLanguages = records.Select(r => new CsvItem(int.Parse(r.index), r.spoken_languages)).ToList();


                var finalMovieGenres = ProcessSpaceSeperatedStrings(genres).Select(g => new MovieSubItem<MovieGenre>(g.Index, new MovieGenre { Genre = new Genre { Name = g.Value } })).ToList();
                var finalMovieActors = ProcessSpaceSeperatedStrings(actors).Select(a => new MovieSubItem<MovieCast>(a.Index, new MovieCast { Actor = new Actor { Name = a.Value } })).ToList();
                var finalMovieKeywords = ProcessSpaceSeperatedStrings(keywords).Select(k => new MovieSubItem<MovieKeyword>(k.Index, new MovieKeyword { Keyword = new Keyword { Name = k.Value } })).ToList();

                var finalMovieCountries = ProcessJsonStrings(productionCountries).Select(c => new MovieSubItem<MovieProductionCountry>(c.Index, new MovieProductionCountry { ProductionCountry = new ProductionCountry { Name = c.Value } })).ToList();
                var finalMovieCompanies = ProcessJsonStrings(productionCompanies).Select(c => new MovieSubItem<MovieProductionCompany>(c.Index, new MovieProductionCompany { ProductionCompany = new ProductionCompany { Name = c.Value } })).ToList();
                var finalMovieSpokenLanguages = ProcessJsonStrings(spokenLanguages).Select(s => new MovieSubItem<MovieSpokenLanguage>(s.Index, new MovieSpokenLanguage { SpokenLanguage = new SpokenLanguage { Name = s.Value } })).ToList();
                List<Movie> movies = [];


                foreach (var m in records)
                {

                    try
                    {
                        var id = int.Parse(m.index);
                        var popularity = double.Parse(m.popularity);
                        var releaseDate = DateTime.Parse(m.release_date);
                        var revenue = double.Parse(m.revenue);
                        var runtime = int.Parse(m.runtime);
                        var voteAverage = double.Parse(m.vote_average);
                        var voteCount = int.Parse(m.vote_count);
                        var budget = double.Parse(m.budget);

                        var fmg = finalMovieGenres.Where(f => f.MovieId == id).Select(f => f.Value).ToList();
                        var fma = finalMovieActors.Where(f => f.MovieId == id).Select(f => f.Value).ToList();
                        var fmk = finalMovieKeywords.Where(f => f.MovieId == id).Select(f => f.Value).ToList();

                        var fmsl = finalMovieSpokenLanguages.Where(f => f.MovieId == id).Select(f => f.Value).ToList();
                        var fmc = finalMovieCompanies.Where(f => f.MovieId == id).Select(f => f.Value).ToList();
                        var fmcn = finalMovieCountries.Where(f => f.MovieId == id).Select(f => f.Value).ToList();

                        var movie = new Movie
                        {
                            Title = m.title,
                            Overview = m.overview,
                            Budget = budget,
                            Homepage = m.homepage,
                            Popularity = popularity,
                            ReleaseDate = releaseDate,
                            Revenue = revenue,
                            Runtime = runtime,
                            Status = m.status,
                            Tagline = m.tagline,
                            VoteAverage = voteAverage,
                            VoteCount = voteCount,
                            Director = new Actor { Name = m.director },
                            MovieProductionCompanies = fmc,
                            MovieProductionCountries = fmcn,
                            MovieSpokenLanguages = fmsl,
                            MovieCasts = fma,
                            MovieGenres = fmg,
                            MovieKeywords = fmk
                        };
                        await context.Movies.AddAsync(movie);
                        movies.Add(movie);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }
                //await context.Movies.AddRangeAsync(movies);
                var rows = await context.SaveChangesAsync();
                return rows == movies.Count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<bool> DeleteAllEmbeddings(EmbeddingsType type)
        {
            
            int count = 0;
            if (type == EmbeddingsType.jina)
            {
                var movieEmbeddings = await context.MovieEmbeddings.ToListAsync();
                count = movieEmbeddings.Count;
                if (movieEmbeddings.Count > 0)
                {
                    context.MovieEmbeddings.RemoveRange(movieEmbeddings);
                }
            }
            else
            {
                var movieEmbeddings = await context.MovieEmbeddingsCombined.ToListAsync();
                count = movieEmbeddings.Count;
                if (movieEmbeddings.Count > 0)
                {
                    context.MovieEmbeddingsCombined.RemoveRange(movieEmbeddings);
                }
            }

            var rows = await context.SaveChangesAsync();
            return rows == count;
        }

        public async Task<bool> UpdateEmbeddings(List<MovieEmbeddingsResult> results, EmbeddingsType type)
        {
            var ids = results.Select(r => r.MovieId).ToList();
            

            if (type == EmbeddingsType.jina)
            {
                var movieEmbeddings = await context.MovieEmbeddings.Where(m => ids.Contains(m.MovieId)).ToListAsync();

                if (movieEmbeddings.Count > 0)
                {
                    context.MovieEmbeddings.RemoveRange(movieEmbeddings);
                }

                movieEmbeddings = results.Select(r => new MovieEmbedding()
                {

                    MovieId = r.MovieId,
                    Budget = ToJsonString(r.MovieEmbeddings, 0),
                    Homepage = ToJsonString(r.MovieEmbeddings, 1),
                    Title = ToJsonString(r.MovieEmbeddings, 2),
                    Overview = ToJsonString(r.MovieEmbeddings, 3),
                    Popularity = ToJsonString(r.MovieEmbeddings, 4),
                    Status = ToJsonString(r.MovieEmbeddings, 5),
                    Tagline = ToJsonString(r.MovieEmbeddings, 6),
                    VoteAverage = ToJsonString(r.MovieEmbeddings, 7),
                    VoteCount = ToJsonString(r.MovieEmbeddings, 8),
                    DirectorName = ToJsonString(r.MovieEmbeddings, 9),
                    Genres = ToJsonString(r.MovieEmbeddings, 10),
                    Casts = ToJsonString(r.MovieEmbeddings, 11),
                    Keywords = ToJsonString(r.MovieEmbeddings, 12),
                    ProductionCompanies = ToJsonString(r.MovieEmbeddings, 13),
                    ProductionCountries = ToJsonString(r.MovieEmbeddings, 14),
                    SpokenLanguages = ToJsonString(r.MovieEmbeddings, 15),
                }).ToList();

                context.MovieEmbeddings.AddRange(movieEmbeddings);
            }
            else
            {

                var movieEmbeddings = await context.MovieEmbeddingsCombined.Where(m => ids.Contains(m.MovieId)).ToListAsync();

                if (movieEmbeddings.Count > 0)
                {
                    context.MovieEmbeddingsCombined.RemoveRange(movieEmbeddings);
                }

                movieEmbeddings = results.Select(r => new MovieEmbeddingCombined()
                {
                    MovieId = r.MovieId,
                    FullTextEmbeddings = ToJsonString(r.MovieEmbeddings, 0),
                    
                }).ToList();

                context.MovieEmbeddingsCombined.AddRange(movieEmbeddings);

            }

            var rows = await context.SaveChangesAsync();
            return rows > 0;
        }


        private static string ToJsonString(float[][] array, int index)
        {
            return JsonConvert.SerializeObject(array[index]);
        }

        private static List<CsvItem> ProcessSpaceSeperatedStrings(List<CsvItem> items)
        {
            List<CsvItem> returnItems = [];
            foreach (var item in items)
            {
                int movieId = item.Index;
                var values = item.Value.Split(' ').ToList();
                if (values.Count > 0)
                {
                    foreach (var value in values)
                    {
                        returnItems.Add(new CsvItem(movieId, value));
                    }
                }
            }
            return returnItems;
        }

        private static List<CsvItem> ProcessJsonStrings(List<CsvItem> items)
        {
            var returnItems = new List<CsvItem>();

            foreach (var item in items)
            {
                int movieId = item.Index;

                // Deserialize into JArray for flexibility
                var values = JsonConvert.DeserializeObject<JArray>(item.Value);

                if (values is { Count: > 0 })
                {
                    foreach (var value in values)
                    {
                        // Safely extract "name" property if it exists
                        var name = value["name"]?.ToString();
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            returnItems.Add(new CsvItem(movieId, name));
                        }
                    }
                }
            }

            return returnItems;
        }

    }

    public record CsvItem(int Index, string Value);

    public class MovieSubItem<T>(int movieId, T value)
    {
        public int MovieId { get; private set; } = movieId;
        public T Value { get; private set; } = value;

    }



}
