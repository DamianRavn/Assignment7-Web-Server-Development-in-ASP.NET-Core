using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieCharactersAPI.Models
{
    public class Character
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [StringLength(100)]
        public string Alias { get; set; }
        [Required, StringLength(100)]
        public string Gender { get; set; }
        [StringLength(200)]
        public string ImageURL { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
