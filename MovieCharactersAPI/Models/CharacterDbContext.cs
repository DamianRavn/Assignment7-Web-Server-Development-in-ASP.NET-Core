using Microsoft.EntityFrameworkCore;

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
    }
}
