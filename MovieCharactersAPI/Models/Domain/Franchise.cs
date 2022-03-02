using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieCharactersAPI.Models
{
    public class Franchise
    {
        public int Id { get; set; }
        [Required , StringLength(100)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
