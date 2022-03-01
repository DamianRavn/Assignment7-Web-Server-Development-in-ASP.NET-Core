using Microsoft.EntityFrameworkCore;
using MovieCharactersAPI.Models;

namespace MovieCharactersAPI.Models
{
    public class CharacterDbContext : DbContext
    {
        public DbSet<Character> Character { get; set; }

        public CharacterDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<MovieCharactersAPI.Models.Movie> Movie { get; set; }

        public DbSet<MovieCharactersAPI.Models.Franchise> Franchise { get; set; }
    }
}
