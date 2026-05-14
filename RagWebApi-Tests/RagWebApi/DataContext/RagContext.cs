using Microsoft.EntityFrameworkCore;
using RagWebApi.Models;

namespace RagWebApi.DataContext
{
    public class RagContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<DataDocument> DataDocuments { get; set; }
        public DbSet<ImageMetadata> ImageMetadatas { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<MovieKeyword> MovieKeywords { get; set; }
        public DbSet<MovieCast> MovieCasts { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<ProductionCompany> ProductionCompanies { get; set; }
        public DbSet<MovieProductionCompany> MovieProductionCompanies { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public DbSet<MovieEmbedding> MovieEmbeddings { get; set; }

        public DbSet<MovieEmbeddingCombined> MovieEmbeddingsCombined { get; set; }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<string>().HaveMaxLength(256);

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DataDocument>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.Text).HasColumnType("NVARCHAR(MAX)");
                etb.Property(x => x.Title).HasMaxLength(1000);
                etb.Property(x => x.DescriptionEmbedding).HasColumnType("NVARCHAR(MAX)");
                etb.ToTable("DataDocuments");
            });

            modelBuilder.Entity<ImageMetadata>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.AIInsights).HasMaxLength(1000);
                etb.Property(x => x.ImageEmbedding).HasColumnType("NVARCHAR(MAX)");
                etb.Property(x => x.InsightEmbedding).HasColumnType("NVARCHAR(MAX)");
                etb.ToTable("ImageMetadatas");
            });

            modelBuilder.Entity<Movie>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.Title).IsRequired().HasMaxLength(1000);
                etb.Property(x => x.Overview).HasMaxLength(3000);
                etb.Property(x => x.Tagline).HasMaxLength(1000);
                etb.ToTable("Movies");

                // FK: DirectorId → Actor.Id
                etb.HasOne(x => x.Director)
                   .WithMany(x => x.DirectedMovies)
                   .HasForeignKey(x => x.DirectorId)
                   .OnDelete(DeleteBehavior.Restrict);

                etb.HasOne(x => x.MovieEmbedding)
                   .WithOne(x => x.Movie)
                   .HasForeignKey<Movie>(x => x.EmbeddingsId)
                   .OnDelete(DeleteBehavior.Restrict);

            });

            modelBuilder.Entity<MovieEmbedding>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.ToTable("MovieEmbeddings");

                // Foreign key to Movie
                etb.HasOne(x => x.Movie)
                   .WithOne(m => m.MovieEmbedding) //if you add inverse nav
                   .HasForeignKey<MovieEmbedding>(x => x.MovieId);

                // Configure all string fields as NVARCHAR(MAX)
                etb.Property(x => x.Title).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.Overview).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.Genres).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.Casts).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.Keywords).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.ProductionCompanies).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.ProductionCountries).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.SpokenLanguages).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.Status).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.Budget).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.DirectorName).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.VoteCount).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.VoteAverage).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.Homepage).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.Tagline).HasColumnType("NVARCHAR(MAX)").IsRequired();
                etb.Property(x => x.Popularity).HasColumnType("NVARCHAR(MAX)").IsRequired();
            });

            modelBuilder.Entity<MovieEmbeddingCombined>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.ToTable("MovieEmbeddingsCombined");

                // Foreign key to Movie
                etb.HasOne(x => x.Movie)
                   .WithOne(m => m.CombinedMovieEmbedding) //if you add inverse nav
                   .HasForeignKey<MovieEmbeddingCombined>(x => x.MovieId);

                // Configure all string fields as NVARCHAR(MAX)
                etb.Property(x => x.FullTextEmbeddings).HasColumnType("NVARCHAR(MAX)").IsRequired();
                
            });

            // Genres
            modelBuilder.Entity<Genre>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.Name).IsRequired().HasMaxLength(200);
                etb.ToTable("Genres");
            });

            // Actors
            modelBuilder.Entity<Actor>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.Name).IsRequired().HasMaxLength(200);
                etb.ToTable("Actors");
            });

            // Keywords
            modelBuilder.Entity<Keyword>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.Name).IsRequired().HasMaxLength(200);
                etb.ToTable("Keywords");
            });
            // ProductionCompany
            modelBuilder.Entity<ProductionCompany>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.Name).IsRequired().HasMaxLength(300);
                etb.ToTable("ProductionCompanies");
            });

            // ProductionCountry
            modelBuilder.Entity<ProductionCountry>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.Name).IsRequired().HasMaxLength(300);
                etb.ToTable("ProductionCountries");
            });
            // SpokenLanguage
            modelBuilder.Entity<SpokenLanguage>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.Name).IsRequired().HasMaxLength(300);
                etb.ToTable("SpokenLanguages");
            });
            // Votes
            modelBuilder.Entity<Vote>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.Property(x => x.Like).IsRequired();
                etb.ToTable("Votes");

                etb.HasOne(x => x.Movie)
                   .WithMany(x => x.Votes)
                   .HasForeignKey(x => x.MovieId);
            });

            // MovieKeywords (junction table)
            modelBuilder.Entity<MovieKeyword>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.ToTable("MovieKeywords");

                etb.HasOne(x => x.Keyword)
                   .WithMany(x => x.MovieKeywords)
                   .HasForeignKey(x => x.KeywordId);

                etb.HasOne(x => x.Movie)
                   .WithMany(x => x.MovieKeywords)
                   .HasForeignKey(x => x.MovieId);
            });

            // MovieCast (junction table)
            modelBuilder.Entity<MovieCast>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.ToTable("MovieCasts");

                etb.HasOne(x => x.Actor)
                   .WithMany(x => x.MovieCasts)
                   .HasForeignKey(x => x.ActorId);

                etb.HasOne(x => x.Movie)
                   .WithMany(x => x.MovieCasts)
                   .HasForeignKey(x => x.MovieId);
            });

            // MovieGenre (junction table)
            modelBuilder.Entity<MovieGenre>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.ToTable("MovieGenres");

                etb.HasOne(x => x.Genre)
                   .WithMany(x => x.MovieGenres)
                   .HasForeignKey(x => x.GenreId);

                etb.HasOne(x => x.Movie)
                   .WithMany(x => x.MovieGenres)
                   .HasForeignKey(x => x.MovieId);
            });



            // MovieProductionCompanies (junction table)
            modelBuilder.Entity<MovieProductionCompany>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.ToTable("MovieProductionCompanies");

                etb.HasOne(x => x.ProductionCompany)
                   .WithMany(x => x.MovieProductionCompanies)
                   .HasForeignKey(x => x.ProductionCompanyId);

                etb.HasOne(x => x.Movie)
                   .WithMany(x => x.MovieProductionCompanies)
                   .HasForeignKey(x => x.MovieId);
            });

            // MovieProductionCountries (junction table)
            modelBuilder.Entity<MovieProductionCountry>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.ToTable("MovieProductionCountries");

                etb.HasOne(x => x.ProductionCountry)
                   .WithMany(x => x.MovieProductionCountries)
                   .HasForeignKey(x => x.ProductionCountryId);

                etb.HasOne(x => x.Movie)
                   .WithMany(x => x.MovieProductionCountries)
                   .HasForeignKey(x => x.MovieId);
            });

            // MovieSpokenLanguages (junction table)
            modelBuilder.Entity<MovieSpokenLanguage>(etb =>
            {
                etb.HasKey(x => x.Id);
                etb.Property(x => x.Id).UseIdentityColumn().ValueGeneratedOnAdd();
                etb.ToTable("MovieSpokenLanguages");

                etb.HasOne(x => x.SpokenLanguage)
                   .WithMany(x => x.MovieSpokenLanguages)
                   .HasForeignKey(x => x.SpokenLanguageId);

                etb.HasOne(x => x.Movie)
                   .WithMany(x => x.MovieSpokenLanguages)
                   .HasForeignKey(x => x.MovieId);
            });

        }
    }


}
