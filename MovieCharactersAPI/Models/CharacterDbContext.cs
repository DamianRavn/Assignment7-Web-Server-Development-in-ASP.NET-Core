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

            modelBuilder.Entity<Character>().HasData(new Movie() { Id = 1, Title = "The Avengers", Genre = "Action", Year = 2012, Director = "Joss Whedon", FranchiseId = 1 });

            modelBuilder.Entity<Character>().HasData(new Franchise() { Id = 1, Name = "MCU", Description = "Marvel's cinematic universe." });

            // Seeding m2m Character-Movie.
            modelBuilder.Entity<Character>()
                .HasMany(p => p.Movies)
                .WithMany(m => m.Characters)
                .UsingEntity<Dictionary<string, object>>(
                    "CoachCertification",
                    r => r.HasOne<Movie>().WithMany().HasForeignKey("MovieId"),
                    l => l.HasOne<Character>().WithMany().HasForeignKey("CharacterId"),
                    je =>
                    {
                        je.HasKey("CharacterId", "MovieId");
                        je.HasData(
                            new { CharacterId = 1, MovieId = 1 },
                            new { CharacterId = 1, MovieId = 2 }
                        );
                    });
        }
    }
}
