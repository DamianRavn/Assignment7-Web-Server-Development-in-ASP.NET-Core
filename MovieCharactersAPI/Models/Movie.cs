using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieCharactersAPI.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Title { get; set; }
        [Required, StringLength(100)]
        public string Genre { get; set; }
        [Required]
        public int Year { get; set; }
        [Required,StringLength(100)]
        public string Director { get; set; }
        [StringLength(200)]
        public string ImageURL { get; set; }
        [StringLength(200)]
        public string TrailerURL { get; set; }
        public int FranchiseId { get; set; }
        public Franchise Franchise { get; set; }
        public ICollection<Character> Characters { get; set; }
    }
}
