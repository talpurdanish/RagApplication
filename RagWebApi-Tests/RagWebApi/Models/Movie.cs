using System.ComponentModel.DataAnnotations;

namespace RagWebApi.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        public double Budget { get; set; }
        public string Homepage { get; set; } = string.Empty;
        [Required]
        [MaxLength(1000)]
        public string Title { get; set; } = string.Empty;
        [MaxLength(3000)]
        public string Overview { get; set; } = string.Empty;
        public double Popularity { get; set; }
        public double Revenue { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Runtime { get; set; }
        public string Status { get; set; } = string.Empty;
        [MaxLength(1000)]
        public string Tagline { get; set; } = string.Empty;
        public double VoteAverage { get; set; }
        public int VoteCount { get; set; }
        public int DirectorId { get; set; }

        public int EmbeddingsId { get; set; }
        public virtual MovieEmbedding? MovieEmbedding { get; set; }
        public int CombinedEmbeddingsId { get; set; }

        public virtual MovieEmbeddingCombined? CombinedMovieEmbedding { get; set; }

        // Navigation collections
        public virtual Actor? Director { get; set; }
        public virtual ICollection<MovieGenre> MovieGenres { get; set; } = [];
        public virtual ICollection<MovieCast> MovieCasts { get; set; } = [];
        public virtual ICollection<MovieKeyword> MovieKeywords { get; set; } = [];
        public virtual ICollection<MovieProductionCompany> MovieProductionCompanies { get; set; } = [];
        public virtual ICollection<MovieProductionCountry> MovieProductionCountries { get; set; } = [];
        public virtual ICollection<MovieSpokenLanguage> MovieSpokenLanguages { get; set; } = [];
        public virtual ICollection<Vote> Votes { get; set; } = [];



        public string[] ToStringArray()
        {
            var genresString = string.Join(" ", MovieGenres.Select(g => g.Genre == null ? "" : g.Genre.Name).ToArray());
            var castsString = string.Join(" ", MovieCasts.Select(g => g.Actor == null ? "" : g.Actor.Name).ToArray());
            var keywordsString = string.Join(" ", MovieKeywords.Select(g => g.Keyword == null ? "" : g.Keyword.Name).ToArray());
            var companiesString = string.Join(" ", MovieProductionCompanies.Select(g => g.ProductionCompany == null ? "" : g.ProductionCompany.Name).ToArray());
            var countriesString = string.Join(" ", MovieProductionCountries.Select(g => g.ProductionCountry == null ? "" : g.ProductionCountry.Name).ToArray());
            var languagesString = string.Join(" ", MovieSpokenLanguages.Select(g => g.SpokenLanguage == null ? "" : g.SpokenLanguage.Name).ToArray());

            return [
                $"{Budget}",
                Homepage,
                Title,
                Overview,
                $"{Popularity}",
                Status,
                Tagline,
                $"{VoteAverage}",
                $"{VoteCount}",
                Director?.Name??"",
                genresString,
                castsString,
                keywordsString,
                companiesString,
                countriesString,
                languagesString
            ];
        }

        public override string ToString()
        {
            var genresString = string.Join(" ", MovieGenres.Select(g => g.Genre == null ? "" : g.Genre.Name).ToArray());
            var castsString = string.Join(" ", MovieCasts.Select(g => g.Actor == null ? "" : g.Actor.Name).ToArray());
            var keywordsString = string.Join(" ", MovieKeywords.Select(g => g.Keyword == null ? "" : g.Keyword.Name).ToArray());
            var companiesString = string.Join(" ", MovieProductionCompanies.Select(g => g.ProductionCompany == null ? "" : g.ProductionCompany.Name).ToArray());
            var countriesString = string.Join(" ", MovieProductionCountries.Select(g => g.ProductionCountry == null ? "" : g.ProductionCountry.Name).ToArray());
            var languagesString = string.Join(" ", MovieSpokenLanguages.Select(g => g.SpokenLanguage == null ? "" : g.SpokenLanguage.Name).ToArray());

            return
                $" Budget: {Budget}" +
                $" Homepage: {Homepage}" +
                $" Title: {Title}" +
                $" Overview: {Overview}" +
                $" Popularity: {Popularity}" +
                $" Status: {Status}" +
                $" Tagline: {Tagline}" +
                $" VoteAverage:{VoteAverage}" +
                $" VoteCount: {VoteCount}" +
                $" Director: {Director?.Name ?? ""}" +
                $" Genres: {genresString}" +
                $" Actors: {castsString}" +
                $" Keywords: {keywordsString}" +
                $" Production Companies: {companiesString}" +
                $" Production Countries: {countriesString}" +
                $" Spoken Languages: {languagesString}";
        }
    }

    public class MovieEmbedding
    {

        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public virtual Movie? Movie { get; set; }

        public required string Budget { get; set; }
        public required string Homepage { get; set; }
        public required string Title { get; set; }
        public required string Overview { get; set; }
        public required string Popularity { get; set; }
        public required string Status { get; set; }
        public required string Tagline { get; set; }
        public required string VoteAverage { get; set; }
        public required string VoteCount { get; set; }
        public required string DirectorName { get; set; }
        public required string Genres { get; set; }
        public required string Casts { get; set; }
        public required string Keywords { get; set; }
        public required string ProductionCompanies { get; set; }
        public required string ProductionCountries { get; set; }
        public required string SpokenLanguages { get; set; }


    }

    public class MovieEmbeddingCombined {

        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        public virtual Movie? Movie { get; set; }
        public required string FullTextEmbeddings { get; set; }
    }


    public class Genre
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<MovieGenre> MovieGenres { get; set; } = [];
    }

    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<MovieCast> MovieCasts { get; set; } = [];
        public virtual ICollection<Movie> DirectedMovies { get; set; } = [];
    }

    public class Keyword
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<MovieKeyword> MovieKeywords { get; set; } = [];

    }

    public class MovieKeyword
    {
        [Key]
        public int Id { get; set; }
        public int KeywordId { get; set; }
        public int MovieId { get; set; }
        public virtual Keyword? Keyword { get; set; }
        public virtual Movie? Movie { get; set; }

    }

    public class MovieCast
    {
        [Key]
        public int Id { get; set; }
        public int ActorId { get; set; }
        public int MovieId { get; set; }

        public virtual Actor? Actor { get; set; }
        public virtual Movie? Movie { get; set; }

    }

    public class MovieGenre
    {

        [Key]
        public int Id { get; set; }
        public int GenreId { get; set; }
        public int MovieId { get; set; }
        public virtual Genre? Genre { get; set; }
        public virtual Movie? Movie { get; set; }

    }

    public class ProductionCompany
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<MovieProductionCompany> MovieProductionCompanies { get; set; } = [];

    }

    public class MovieProductionCompany
    {

        [Key]
        public int Id { get; set; }
        public int ProductionCompanyId { get; set; }
        public int MovieId { get; set; }
        public virtual ProductionCompany? ProductionCompany { get; set; }
        public virtual Movie? Movie { get; set; }

    }

    public class Vote
    {
        [Key]
        public int Id { get; set; }
        public int MovieId { get; set; }
        [Required]
        public bool Like { get; set; }
        public virtual Movie? Movie { get; set; }


    }

    public class ProductionCountry
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<MovieProductionCountry> MovieProductionCountries { get; set; } = [];
    }

    public class MovieProductionCountry
    {

        [Key]
        public int Id { get; set; }
        public int ProductionCountryId { get; set; }
        public int MovieId { get; set; }
        public virtual ProductionCountry? ProductionCountry { get; set; }
        public virtual Movie? Movie { get; set; }

    }

    public class SpokenLanguage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<MovieSpokenLanguage> MovieSpokenLanguages { get; set; } = [];

    }

    public class MovieSpokenLanguage
    {

        [Key]
        public int Id { get; set; }
        public int SpokenLanguageId { get; set; }
        public int MovieId { get; set; }
        public virtual SpokenLanguage? SpokenLanguage { get; set; }
        public virtual Movie? Movie { get; set; }

    }
}
