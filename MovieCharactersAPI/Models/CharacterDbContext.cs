using Microsoft.EntityFrameworkCore;
using MovieCharactersAPI.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;


namespace MovieCharactersAPI.Models
{
    public class CharacterDbContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Franchise> Franchises { get; set; }


        public CharacterDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seeding data
            modelBuilder.Entity<Character>().HasData(new Character() { Id = 1, Name = "Thor", Alias = "Lord of Thunder", Gender = "Male"});
            modelBuilder.Entity<Character>().HasData(new Character() { Id = 2, Name = "Ironman", Alias = "Tony Stark", Gender = "Male" });
            modelBuilder.Entity<Character>().HasData(new Character() { Id = 3, Name = "Black Widow", Alias = "Natasha Romanoff", Gender = "Female" });
            modelBuilder.Entity<Character>().HasData(new Character() { Id = 4, Name = "Spiderman", Alias = "Peter Parker", Gender = "Male" });
            modelBuilder.Entity<Character>().HasData(new Character() { Id = 5, Name = "Batman", Alias = "Bruce Wayne", Gender = "Male" });

            modelBuilder.Entity<Franchise>().HasData(new Franchise() { Id = 1, Name = "MCU", Description = "Marvel's cinematic universe." });
            modelBuilder.Entity<Franchise>().HasData(new Franchise() { Id = 2, Name = "DNT", Description = "Dark Knight Trilogy" });
            modelBuilder.Entity<Franchise>().HasData(new Franchise() { Id = 3, Name = "TCU", Description = "Titanic's cinematic universe." });

            modelBuilder.Entity<Movie>().HasData(new Movie() { Id = 1, Title = "The Avengers", Genre = "Action", Year = 2012, Director = "Joss Whedon", FranchiseId = 1 });
            modelBuilder.Entity<Movie>().HasData(new Movie() { Id = 2, Title = "Avengers: Endgame", Genre = "Action", Year = 2019, Director = "Anthony Russo, Joe Russo", FranchiseId = 1 });
            modelBuilder.Entity<Movie>().HasData(new Movie() { Id = 3, Title = "The Dark Knight", Genre = "Action", Year = 2008, Director = "Christopher Nolan" });
            modelBuilder.Entity<Movie>().HasData(new Movie() { Id = 4, Title = "Titanic", Genre = "Action", Year = 1997, Director = "James Camera" });

            // Seeding m2m Character-Movie.
            modelBuilder.Entity<Character>()
                .HasMany(p => p.Movies)
                .WithMany(m => m.Characters)
                .UsingEntity<Dictionary<string, object>>(
                    "CharacterMovie",
                    r => r.HasOne<Movie>().WithMany().HasForeignKey("MovieId"),
                    l => l.HasOne<Character>().WithMany().HasForeignKey("CharacterId"),
                    je =>
                    {
                        je.HasKey("CharacterId", "MovieId");
                        je.HasData(
                            new { CharacterId = 1, MovieId = 1 },
                            new { CharacterId = 1, MovieId = 2 },
                            new { CharacterId = 2, MovieId = 1 },
                            new { CharacterId = 2, MovieId = 2 },
                            new { CharacterId = 3, MovieId = 1 },
                            new { CharacterId = 3, MovieId = 2 },
                            new { CharacterId = 4, MovieId = 2 },
                            new { CharacterId = 5, MovieId = 3 }
                        );
                    });
        }
    }
}
