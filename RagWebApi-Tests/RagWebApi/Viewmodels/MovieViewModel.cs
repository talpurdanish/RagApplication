using RagWebApi.Models;

namespace RagWebApi.Viewmodels
{
    public class MovieViewModel
    {
        public int Id { get; set; }
        public double Budget { get; set; }
        public string Homepage { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Overview { get; set; } = string.Empty;
        public double Popularity { get; set; }
        public DateTime ReleaseDate { get; set; }
        public double Revenue { get; set; }
        public int Runtime { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Tagline { get; set; } = string.Empty;
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public int DirectorId { get; set; }
        public string Director { get; set; } = string.Empty;
        public string? Embeddings { get; set; }

        public List<Actor> Actors { get; set; } = [];
        public List<Genre> Genres { get; set; } = [];
        public List<Keyword> Keywords { get; set; } = [];
        public List<ProductionCompany> Companies { get; set; } = [];
        public List<ProductionCountry> Countries { get; set; } = [];
        public List<SpokenLanguage> SpokenLanguages { get; set; } = [];

    }

    public class MiniMovieViewModel {
        public int Id { get; set; }
        public List<Actor> Actors { get; set; } = [];
        public List<Keyword> Keywords { get; set; } = [];
        public string Title { get; set; } = string.Empty;
        public double Similarity { get; set; }

    }

    public class PagedResults {


        public IEnumerable<MiniMovieViewModel> MovieViewModels { get; set; } = [];
        public int CurrentPage { get; set; } = 1;
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int? NextPage { get; set; }
        public int? PrevPage { get; set; }

        public PagedResults(IEnumerable<MiniMovieViewModel> data, int currentPage, int totalRecords, int pageSize)
        {
            MovieViewModels = data;
            CurrentPage = currentPage;
            TotalRecords = totalRecords;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            NextPage = CurrentPage < TotalPages ? CurrentPage + 1 : (int?)null;
            PrevPage = CurrentPage > 1 ? CurrentPage - 1 : (int?)null;
        }

        public PagedResults(IEnumerable<MiniMovieViewModel> data)
        {
            var dataCount = data.Count();
            MovieViewModels = data;
            CurrentPage = 1;
            TotalRecords = dataCount;
            PageSize = data.Count();
            TotalPages = 1;
            NextPage = null;
            PrevPage = null;
        }

    }

    public class WeightedQuery {

        public string Query { get; set; } = string.Empty;
        public double Budget { get; set; }
        public double Homepage { get; set; }
        public double Title { get; set; }
        public double Overview { get; set; }
        public double Popularity { get; set; }
        public double Status { get; set; }
        public double Tagline { get; set; }
        public double VoteAverage { get; set; }
        public double VoteCount { get; set; }
        public double DirectorName { get; set; }
        public double Genres { get; set; }
        public double Casts { get; set; }
        public double Keywords { get; set; }
        public double ProductionCompanies { get; set; }
        public double ProductionCountries { get; set; }
        public double SpokenLanguages { get; set; }

        public int Type { get; set; }

    }
}
